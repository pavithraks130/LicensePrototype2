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

        /// <summary>
        /// Get the list  of License for the approval based on the team list.
        /// </summary>
        /// <returns></returns>
        public ActionResult LicenseApproval()
        {
            GetTeamList();
            return View();
        }
        /// <summary>
        ///  Gets the list of team  base on the Role User ID   and role who has logged in
        /// </summary>
        public void GetTeamList()
        {
            ViewBag.SelectedTeamId = LicenseSessionState.Instance.SelectedTeam.Id;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                ViewBag.TeamList = LicenseSessionState.Instance.TeamList;
            else
            {
                // Get the list of team to which the user is the Admin
                List<Team> teamList = new List<Team>();
                foreach (var team in LicenseSessionState.Instance.TeamList)
                {
                    if (team.TeamMembers.Any(t => t.IsAdmin == true && t.InviteeUserId == LicenseSessionState.Instance.User.UserId))
                        teamList.Add(team);
                }
                ViewBag.TeamList = teamList;
            }
        }

        /// <summary>
        /// Get Action method to return view with  the License Request list based on the Team Id.
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public ActionResult LicenseApprovalByTeam(int teamId)
        {
            List<UserLicenseRequest> requestList = new List<UserLicenseRequest>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.GetAsync("api/UserLicense/GetRequestByTeam/" + teamId).Result;
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

        /// <summary>
        /// POST action to update the Approve or reject status for the selected License Request.
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="status"></param>
        /// <param name="selectLicenseRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LicenseApprovalByTeam(string comment, string status, params string[] selectLicenseRequest)
        {
            List<UserLicenseRequest> licReqList = new List<UserLicenseRequest>();
            foreach (var id in selectLicenseRequest)
            {
                UserLicenseRequest userlicReq = new UserLicenseRequest();
                userlicReq.Id = Convert.ToInt32(id);
                userlicReq.Comment = comment;
                userlicReq.ApprovedBy = LicenseSessionState.Instance.User.UserName;
                switch (status)
                {
                    case "Approve": userlicReq.IsApproved = true; break;
                    case "Reject": userlicReq.IsRejected = true; break;
                }
                licReqList.Add(userlicReq);
            }
            if (licReqList.Count > 0)
            {
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
                var response = client.PostAsJsonAsync("api/UserLicense/ApproveReject", licReqList).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                    ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
                    GetTeamList();
                    return View();
                }
            }
            return RedirectToAction("LicenseApproval");
        }

        /// <summary>
        /// Get actioin, return view with the list of license to assign to single or multiple user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bulkLicenseAdd"></param>
        /// <returns></returns>
        public ActionResult AssignLicense(string userId, bool bulkLicenseAdd)
        {
            ViewBag.UserEmail = "";
            if (!bulkLicenseAdd)
                ViewBag.UserEmail = LicenseSessionState.Instance.SelectedTeam.TeamMembers.FirstOrDefault(t => t.InviteeUserId == userId).InviteeEmail;
            var listdata = GetLicenseListBySubscription(userId, bulkLicenseAdd);
            return View(listdata);
        }

        /// <summary>
        /// POST action to update/map the license to user for the selected products. 
        /// </summary>
        /// <param name="selectedProduct"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignLicense(params string[] selectedProduct)
        {
            var responseData = UpdateLicense(selectedProduct);
            if (!String.IsNullOrEmpty(responseData))
            {
                ModelState.AddModelError("", responseData);
                return View("TeamContainer", "TeamManagement");
            }
            return RedirectToAction("TeamContainer", "TeamManagement");
        }
        
        /// <summary>
        /// Get the Subscription with product list based on the user ID. bulkLicenseAdd is to differentiate the  screen data which is being fetched
        ///  if this is set to false then the  exist user license mapped details will also be fetched along with subscription list.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bulkLicenseAdd"></param>
        /// <returns></returns>
        public IList<Product> GetLicenseListBySubscription(string userId, bool bulkLicenseAdd)
        {
            TempData["UserId"] = userId;
            TempData["CanAddBulk"] = bulkLicenseAdd;
            ViewBag.TeamMember = userId == null ? string.Empty : LicenseSessionState.Instance.User.Email;
            string adminUserId = string.Empty;

            // To get the Super Admin Id for the team if the logged in user is not super admin to get the Subscriptions. Because the Subscriptions
            // are purchased by the super admin not by admin
            if (LicenseSessionState.Instance.SelectedTeam != null)
                adminUserId = LicenseSessionState.Instance.SelectedTeam.AdminId;

            // If the license map is the bulk license Map then only the Product List will be fetched else if the license map is for the
            // single user then along with Product data already assigned products details  will also be fetched  for the user

            IList<Product> productList = null;
            if (bulkLicenseAdd)
                productList = OnPremiseSubscriptionLogic.GetProductsFromSubscription();
            else
                productList = OnPremiseSubscriptionLogic.GetProductsFromSubscription(userId);

            return productList;
        }

        /// <summary>
        /// GET action , returns view with the list of products which is already mapped to the user for removing the mapping
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult RevokeLicense(string userId)
        {
            TempData["UserId"] = userId;
            UserLicenseDetails licDetails = OnPremiseSubscriptionLogic.GetUserLicenseDetails(userId, false, false);
            ViewBag.UserEmail = licDetails.User.Email;
            return View(licDetails.Products);
        }

        /// <summary>
        /// POST Action,  call will update the Data by removing the Product license  by user for the selected products
        /// </summary>
        /// <param name="SelectedSubscription"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RevokeLicense(params string[] selectedproduct)
        {
            var responseData = RevokeLicenseFromUser(selectedproduct);
            if (!String.IsNullOrEmpty(responseData))
            {
                ModelState.AddModelError("", responseData);
                return View("TeamContainer", "TeamManagement");
            }
            return RedirectToAction("TeamContainer", "TeamManagement");
        }
        /// <summary>
        /// POST action, this action used in Bulk License.  Once the user select the Bulk License the license list will be displayed,
        /// once the products are selected then the user will redirected to the users screen for selecting the users.
        /// This action is responsible for listing the Users based on the Team for assigning the license to multiple user.
        /// </summary>
        /// <param name="SelectedSubscription"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SelectUsers(params string[] SelectedSubscription)
        {
            TempData["SelectedSubscription"] = SelectedSubscription;
            List<TeamMember> teamMember = new List<TeamMember>();
            if (LicenseSessionState.Instance.SelectedTeam != null)
                teamMember = LicenseSessionState.Instance.SelectedTeam.TeamMembers.ToList();
            else
                teamMember = new List<TeamMember>();
            return View(teamMember);
        }

        /// <summary>
        /// POST ACtion :  once the user selected and submitted this action will be called to update 
        /// the product License to users using service Call.
        /// </summary>
        /// <param name="SelectedUser"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SelectedUsers(params string[] SelectedUser)
        {
            string[] temp = TempData["SelectedSubscription"] as string[];
            UpdateLicense(temp, "Add", SelectedUser, true);
            return RedirectToAction("TeamContainer", "TeamManagement");
        }

        /// <summary>
        /// Function responsible for calling the  service to update the License to multiple users.
        /// </summary>
        /// <param name="SelectedSubscription"></param>
        /// <param name="action"></param>
        /// <param name="SelectedUserIdList"></param>
        /// <param name="canAddBulkLicense"></param>
        /// <returns></returns>
        public string UpdateLicense(string[] selectedProduct, string action = "Add", string[] SelectedUserIdList = null, bool canAddBulkLicense = false)
        {
            List<User> userList = new List<User>();
            if (canAddBulkLicense)
                userList = SelectedUserIdList.Select(u => new User() { UserId = u }).ToList();
            else
                userList.Add(new User() { UserId = Convert.ToString(TempData["UserId"]) });
           
            // Converting the selected Product to the Product License Object which need to be mapped to the user 
            List<ProductLicense> lstLicData = selectedProduct.ToList().Select(id => new ProductLicense() {ProductId = Convert.ToInt32(id) }).ToList();

            // service call to map the product to license.
            UserLicenseDataMapping mapping = new UserLicenseDataMapping()
            {
                TeamId = LicenseSessionState.Instance.SelectedTeam.Id,
                LicenseDataList = lstLicData,
                UserList = userList
            };

            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.PostAsJsonAsync("api/UserLicense/Create", mapping).Result;
            if (!response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                return response.ReasonPhrase + " - " + obj.Message;
            }
            else
            {
                if (userList.Any(u => u.UserId == LicenseSessionState.Instance.User.UserId))
                {
                    var productDetails = OnPremiseSubscriptionLogic.GetUserLicenseForUser();
                    LicenseSessionState.Instance.UserSubscribedProducts = productDetails;
                }
            }
            return String.Empty;
        }

        /// <summary>  
        /// Function to make service call to revoke single or multple Product License from user by making a service call
        /// </summary>
        /// <param name="SelectedSubscription"></param>
        /// <returns></returns>
        public string RevokeLicenseFromUser(string[] selectedproduct)
        {
            List<User> userList = new List<User>();
            userList.Add(new User() { UserId = Convert.ToString(TempData["UserId"]) });
            // Creation of Product License List based on the Product Selection
            var lstLicData = selectedproduct.ToList().Select(prodId => new ProductLicense() { ProductId = Convert.ToInt32(prodId) }).ToList();            
            // Service call to revoke license from user
            UserLicenseDataMapping mapping = new UserLicenseDataMapping()
            {
                TeamId = LicenseSessionState.Instance.SelectedTeam.Id,
                LicenseDataList = lstLicData,
                UserList = userList
            };
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.PostAsJsonAsync("api/UserLicense/Revoke", mapping).Result;
            if (!response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                return response.ReasonPhrase + " - " + obj.Message;
            }
            return String.Empty;
        }

        /// <summary>
        /// GET Action, Return view  which will list the all product for the  license Request.
        /// </summary>
        /// <returns></returns>
        public ActionResult LicenseRequest()
        {
            string adminId = String.Empty;
            string userId = LicenseSessionState.Instance.User.UserId;
            var listdata = GetLicenseListBySubscription(userId, false);
            return View(listdata);
        }

        /// <summary>
        /// POST Action. Request will be sent to admin for the Product which are Selected. Once the products selected and Form Submitted the 
        /// seleted product list will be posted to POST Action. Selected License Request will be created for the Product and submitted for the 
        /// Approval
        /// </summary>
        /// <param name="SelectedSubscription"></param>
        /// <returns></returns>
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
            // Service call to create the License Request  
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.PostAsJsonAsync("api/UserLicense/LicenseRequest", licReqList).Result;
            if (!response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);

                return View();
            }
            return RedirectToAction("TeamContainer", "TeamManagement");
        }

        /// <summary>
        ///  Get Action, returns the view with the list of License Request with status along with the comment.
        ///  Service call will be made to get the data based on the userid
        /// </summary>
        /// <returns></returns>
        public ActionResult RequestStatus()
        {
            List<UserLicenseRequest> listlic = new List<UserLicenseRequest>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.GetAsync("api/UserLicense/GetRequestStatus/" + LicenseSessionState.Instance.User.UserId).Result;
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