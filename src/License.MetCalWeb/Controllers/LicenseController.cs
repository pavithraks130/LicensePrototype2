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
    [SessionExpire]
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
            GetTeamList();
            return View();
        }

        public void GetTeamList()
        {
            ViewBag.SelectedTeamId = LicenseSessionState.Instance.SelectedTeam.Id;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                ViewBag.TeamList = LicenseSessionState.Instance.TeamList;
            else
            {
                List<Team> teamList = new List<Team>();
                foreach (var team in LicenseSessionState.Instance.TeamList)
                {
                    if (team.TeamMembers.Any(t => t.IsAdmin == true && t.InviteeUserId == LicenseSessionState.Instance.User.UserId))
                        teamList.Add(team);
                }
                ViewBag.TeamList = teamList;
            }
        }

        public ActionResult LicenseApprovalByTeam(int teamId)
        {
            List<UserLicenseRequest> requestList = new List<UserLicenseRequest>();
            var adminId = string.Empty;

            if (LicenseSessionState.Instance.SelectedTeam != null)
                adminId = LicenseSessionState.Instance.SelectedTeam.AdminId;

            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.GetAsync("api/License/GetRequestedLicenseByTeam/" + teamId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(jsonData))
                    requestList = JsonConvert.DeserializeObject<List<UserLicenseRequest>>(jsonData);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return View(requestList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LicenseApprovalByTeam(string comment, string status, params string[] selectLicenseRequest)
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
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
                var response = client.PostAsJsonAsync("api/license/ApproveRejectLicense", licReqList).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                    ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
                    GetTeamList();
                    return View();
                }
            }
            return RedirectToAction("TeamContainer", "TeamManagement");
        }

        public ActionResult MapLicense(string userId, bool bulkLicenseAdd)
        {
            ViewData["UserEmail"] = "";
            if (!bulkLicenseAdd)
                ViewData["UserEmail"] = LicenseSessionState.Instance.SelectedTeam.TeamMembers.FirstOrDefault(t => t.InviteeUserId == userId).InviteeEmail;
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
            if (LicenseSessionState.Instance.SelectedTeam != null)
                adminUserId = LicenseSessionState.Instance.SelectedTeam.AdminId;

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
                return View("TeamContainer", "TeamManagement");
            }
            return RedirectToAction("TeamContainer", "TeamManagement");
        }

        public ActionResult RevokeLicense(string userId)
        {
            TempData["UserId"] = userId;
            UserLicenseDetails licDetails = OnPremiseSubscriptionLogic.GetUserLicenseDetails(userId, false, false);
            ViewData["UserEmail"] = licDetails.User.Email;
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
                return View("TeamContainer", "TeamManagement");
            }
            return RedirectToAction("TeamContainer", "TeamManagement");
        }

        [HttpPost]
        public ActionResult SelectListOfUser(params string[] SelectedSubscription)
        {
            TempData["SelectedSubscription"] = SelectedSubscription;
            List<TeamMember> teamMember = new List<TeamMember>();
            if (LicenseSessionState.Instance.SelectedTeam != null)
                teamMember = LicenseSessionState.Instance.SelectedTeam.TeamMembers.ToList();
            else
                teamMember = new List<TeamMember>();
            return View(teamMember);
        }

        [HttpPost]
        public ActionResult SelectedUsers(params string[] SelectedUser)
        {
            string[] temp = TempData["SelectedSubscription"] as string[];
            UpdateLicense(temp, "Add", SelectedUser, true);
            return RedirectToAction("TeamContainer", "TeamManagement");
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

                LicenseData licData = new LicenseData()
                {
                    UserSubscriptionId = Convert.ToInt32(subscriptionId),
                    ProductId = Convert.ToInt32(prodId)
                };
                lstLicData.Add(licData);
            }
            UserLicenseDataMapping mapping = new UserLicenseDataMapping() { TeamId = LicenseSessionState.Instance.SelectedTeam.Id, LicenseDataList = lstLicData, UserList = userIdList };
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.PostAsJsonAsync("api/License/CreateUserLicence", mapping).Result;
            if (!response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                return response.ReasonPhrase + " - " + obj.Message;
            }
            else
            {
                if (userIdList.Contains(LicenseSessionState.Instance.User.UserId))
                {
                    var subscriptionDetails = OnPremiseSubscriptionLogic.GetUserLicenseForUser();
                    LicenseSessionState.Instance.UserSubscriptionList = subscriptionDetails;
                }
            }

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

                LicenseData licData = new LicenseData()
                {
                    UserSubscriptionId = Convert.ToInt32(subscriptionId),
                    ProductId = Convert.ToInt32(prodId)
                };
                lstLicData.Add(licData);
            }
            UserLicenseDataMapping mapping = new UserLicenseDataMapping() { TeamId = LicenseSessionState.Instance.SelectedTeam.Id, LicenseDataList = lstLicData, UserList = userIdList };
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.PostAsJsonAsync("api/License/RevokeUserLicence", mapping).Result;
            if (!response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                return response.ReasonPhrase + " - " + obj.Message;
            }
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
                    RequestedDate = DateTime.Now.Date,
                    TeamId = LicenseSessionState.Instance.SelectedTeam.Id
                };
                licReqList.Add(req);
            }

            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.PostAsJsonAsync("api/License/RequestLicense", licReqList).Result;
            if (!response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);

                return View();
            }
            return RedirectToAction("TeamContainer", "TeamManagement");
        }

        public ActionResult RequestStatus()
        {
            List<UserLicenseRequest> listlic = new List<UserLicenseRequest>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.GetAsync("api/License/GetLicenseRequestStatus/" + LicenseSessionState.Instance.User.UserId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(jsonData))
                    listlic = JsonConvert.DeserializeObject<List<UserLicenseRequest>>(jsonData);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return View(listlic);
        }
    }
}