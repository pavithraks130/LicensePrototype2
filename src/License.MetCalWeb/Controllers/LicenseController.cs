using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace License.MetCalWeb.Controllers
{
    [Authorize]
    public class LicenseController : Controller
    {
        // GET: License
        public ActionResult Index()
        {
            return View();
        }

        public LicenseController()
        {

        }

        public ActionResult LicenseApproval()
        {
            List<UserLicenseRequest> requestList = new List<UserLicenseRequest>();
            var adminId = string.Empty;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                adminId = LicenseSessionState.Instance.User.UserId;
            else if (LicenseSessionState.Instance.IsAdmin)
                adminId = LicenseSessionState.Instance.AdminId;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.GetAsync("api/License/GetRequestedLicense/" + adminId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(jsonData))
                    requestList = JsonConvert.DeserializeObject<List<UserLicenseRequest>>(jsonData);
            }
            return View(requestList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LicenseApproval(string comment, string status, params string[] selectLicenseRequest)
        {
            List<UserLicenseRequest> licReqList = new List<UserLicenseRequest>();
            foreach (var id in selectLicenseRequest)
            {
                UserLicenseRequest userlicReq = new UserLicenseRequest();
                userlicReq.Id = Convert.ToInt32(id);
                switch (status)
                {
                    case "Approve": userlicReq.IsApproved = true; break;
                    case "Reject": userlicReq.IsRejected = true; break;
                }
                userlicReq.Comment = comment;
                userlicReq.ApprovedBy = LicenseSessionState.Instance.User.UserName;
                licReqList.Add(userlicReq);
            }

            if (licReqList.Count > 0)
            {
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
                var response = client.PostAsJsonAsync("api/license/ApproveRejectLicense", licReqList).Result;
                if (!!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", response.ReasonPhrase);
                    return View();
                }
            }
            return RedirectToAction("TeamContainer", "Team");
        }

        public ActionResult MapLicense(string userId, bool bulkLicenseAdd)
        {

            var listdata = GetLicenseListBySubscription(userId, bulkLicenseAdd);
            return View(listdata);
        }

        public IList<SubscriptionDetails> GetLicenseListBySubscription(string userId, bool bulkLicenseAdd)
        {
            TempData["UserId"] = userId;
            TempData["CanAddBulk"] = bulkLicenseAdd;
            ViewData["TeamMember"] = userId == null ? string.Empty : LicenseSessionState.Instance.User.Email;
            //Logic to get the Subscription details Who are Team Member and Role is assigned as admin by the Super admin
            string adminUserId = string.Empty;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                adminUserId = LicenseSessionState.Instance.User.UserId;
            else
                adminUserId = LicenseSessionState.Instance.AdminId;

            IList<SubscriptionDetails> licenseMapModelList = null;
            if (bulkLicenseAdd)
                licenseMapModelList = OnPremiseSubscriptionLogic.GetSubscription(adminUserId);
            else
                licenseMapModelList = OnPremiseSubscriptionLogic.GetSubscriptionForLicenseMap(userId, adminUserId);
            return licenseMapModelList;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MapLicense(params string[] SelectedSubscription)
        {
            var responseData = UpdateLicense(SelectedSubscription);
            if (!String.IsNullOrEmpty(responseData))
            {
                ModelState.AddModelError("", responseData);
                return View("TeamContainer", "Team");
            }
            return RedirectToAction("TeamContainer", "Team");
        }

        public ActionResult RevokeLicense(string userId)
        {
            TempData["UserId"] = userId;
            UserLicenseDetails licDetails = OnPremiseSubscriptionLogic.GetUserLicenseDetails(userId, false);
            ViewData["TeamMember"] = licDetails.User.Email;
            return View(licDetails.SubscriptionDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RevokeLicense(params string[] SelectedSubscription)
        {
            var responseData = RevokeLicenseFromUser(SelectedSubscription);
            if (!String.IsNullOrEmpty(responseData))
            {
                ModelState.AddModelError("", responseData);
                return View("TeamContainer", "Team");
            }
            return RedirectToAction("TeamContainer", "Team");
        }

        [HttpPost]
        public ActionResult SelectListOfUser(params string[] SelectedSubscription)
        {
            TempData["SelectedSubscription"] = SelectedSubscription;
            var teamMember = LoadTeamMember();
            return View(teamMember);
        }

        private TeamDetails LoadTeamMember()
        {

            string adminId = string.Empty;
            TeamDetails model = null;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                adminId = LicenseSessionState.Instance.User.UserId;
            else
                adminId = LicenseSessionState.Instance.AdminId;
            if (!String.IsNullOrEmpty(adminId))
            {
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
                var response = client.GetAsync("api/TeamMember/all/" + adminId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<TeamDetails>(jsonData);
                }
            }
            if (model == null)
            {
                return null;
            }
            if (model.AcceptedUsers.Count <= 0 || LicenseSessionState.Instance.IsTeamMember)
                return model;
            return model;
        }

        [HttpPost]
        public ActionResult SelectedUsers(params string[] SelectedUser)
        {
            string[] temp = TempData["SelectedSubscription"] as string[];
            var teamMember = LoadTeamMember();
            UpdateLicense(temp, "Add", SelectedUser, true);
            return RedirectToAction("TeamContainer", "Team");
        }


        public string UpdateLicense(string[] SelectedSubscription, string action = "Add", string[] SelectedUserIdList = null, bool canAddBulkLicense = false)
        {
            List<string> userIdList = new List<string>();
            if (canAddBulkLicense)
                foreach (var userId in SelectedUserIdList)
                    userIdList.Add(userId);
            else
                userIdList.Add(Convert.ToString(TempData["UserId"]));

            List<LicenseData> lstLicData = new List<LicenseData>();
            foreach (var data in SelectedSubscription)
            {
                var splitValue = data.Split(new char[] { '-' });
                var prodId = splitValue[0].Split(new char[] { ':' })[1];
                var subscriptionId = splitValue[1].Split(new char[] { ':' })[1];

                LicenseData licData = new LicenseData();
                licData.UserSubscriptionId = Convert.ToInt32(subscriptionId);
                licData.ProductId = Convert.ToInt32(prodId);
                lstLicData.Add(licData);
            }
            UserLicesneDataMapping mapping = new UserLicesneDataMapping() { LicenseDataList = lstLicData, UserList = userIdList };
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.PostAsJsonAsync("api/License/CreateUserLicence", mapping).Result;
            if (!response.IsSuccessStatusCode) return response.ReasonPhrase;
            return String.Empty;
        }

        public string RevokeLicenseFromUser(string[] SelectedSubscription)
        {
            List<string> userIdList = new List<string>();
            userIdList.Add(Convert.ToString(TempData["UserId"]));
            List<LicenseData> lstLicData = new List<LicenseData>();
            foreach (var data in SelectedSubscription)
            {
                var splitValue = data.Split(new char[] { '-' });
                var prodId = splitValue[0].Split(new char[] { ':' })[1];
                var subscriptionId = splitValue[1].Split(new char[] { ':' })[1];

                LicenseData licData = new LicenseData();
                licData.UserSubscriptionId = Convert.ToInt32(subscriptionId);
                licData.ProductId = Convert.ToInt32(prodId);
                lstLicData.Add(licData);
            }
            UserLicesneDataMapping mapping = new UserLicesneDataMapping() { LicenseDataList = lstLicData, UserList = userIdList };
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.PostAsJsonAsync("api/License/RevokeUserLicence", mapping).Result;
            if (!response.IsSuccessStatusCode) return response.ReasonPhrase;
            return String.Empty;
        }


        public ActionResult LicenseRequest()
        {
            string adminId = String.Empty;
            string userId = LicenseSessionState.Instance.User.UserId;
            var listdata = GetLicenseListBySubscription(userId, false);
            return View(listdata);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LicenseRequest(params string[] SelectedSubscription)
        {
            List<UserLicenseRequest> licReqList = new List<UserLicenseRequest>();
            foreach (var data in SelectedSubscription)
            {
                var splitValue = data.Split(new char[] { '-' });
                var prodId = splitValue[0].Split(new char[] { ':' })[1];
                var subscriptionId = splitValue[1].Split(new char[] { ':' })[1];

                UserLicenseRequest req = new UserLicenseRequest()
                {
                    Requested_UserId = LicenseSessionState.Instance.User.UserId,
                    ProductId = Convert.ToInt32(prodId),
                    UserSubscriptionId = Convert.ToInt32(subscriptionId),
                    RequestedDate = DateTime.Now.Date
                };
                licReqList.Add(req);
            }

            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.PostAsJsonAsync("api/License/RequestLicense", licReqList).Result;
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", response.ReasonPhrase);
                return View();
            }
            return RedirectToAction("TeamContainer", "Team");
        }

        public ActionResult RequestStatus()
        {
            List<UserLicenseRequest> listlic = new List<UserLicenseRequest>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.GetAsync("api/License/GetLicenseRequestStatus/" + LicenseSessionState.Instance.User.UserId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(jsonData))
                    listlic = JsonConvert.DeserializeObject<List<UserLicenseRequest>>(jsonData);
            }
            return View(listlic);
        }
    }
}