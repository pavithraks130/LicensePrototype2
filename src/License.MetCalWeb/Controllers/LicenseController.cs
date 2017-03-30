using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.Logic.Common;
using License.Logic.ServiceLogic;
using License.MetCalWeb;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using License.Model;

namespace License.MetCalWeb.Controllers
{
    public class LicenseController : Controller
    {
        private TeamMemberLogic logic = null;
        private UserLogic userLogic = null;
        private UserSubscriptionLogic subscriptionLogic = null;


        // GET: License
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MapLicense(string userId, bool bulkLicenseAdd)
        {
            var listdata = GetLicenseListBySubscription(userId, bulkLicenseAdd);
            return View(listdata);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MapLicense(params string[] SelectedSubscription)
        {
            UpdateRevokeLicense(SelectedSubscription);
            return RedirectToAction("TeamContainer", "Team");
        }

        /// <summary>
        /// It will store selected subscription list in  temp and disply user list for selection
        /// </summary>
        /// <param name="SelectedSubscription"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult SelectListOfUser(params string[] SelectedSubscription)
        {
            TempData["SelectedSubscription"] = SelectedSubscription;
            var teamMember = LoadTeamMember();
            return View(teamMember);
        }

        private TeamModel LoadTeamMember()
        {
            License.Model.UserInviteList inviteList = new UserInviteList();
            string adminId = string.Empty;
            TeamModel model = null;
            if (LicenseSessionState.Instance.IsSuperAdmin)
            {
                adminId = LicenseSessionState.Instance.User.UserId;
            }
            else
                adminId = LicenseSessionState.Instance.AdminId;
            if (!String.IsNullOrEmpty(adminId))
            {
                inviteList = logic.GetUserInviteDetails(adminId);
                model = new TeamModel();
                model.AdminUser = inviteList.AdminUser;
                model.AcceptedUsers = inviteList.AcceptedInvites;
                model.PendinigUsers = inviteList.PendingInvites;
            }
            if (model == null)
                return null;
            if (model.AcceptedUsers.Count <= 0 || Convert.ToBoolean(TempData["IsTeamAdmin"]))
                return model;
            return model;
        }

        [HttpPost]
        public ActionResult SelectedUsers(params string[] SelectedUser)
        {
            string[] temp = TempData["SelectedSubscription"] as string[];
            var teamMember = LoadTeamMember();
            UpdateRevokeLicense(temp, "Add", SelectedUser, true);
            return RedirectToAction("TeamContainer", "Team");
        }

        public List<License.MetCalWeb.Models.LicenseMapModel> GetLicenseListBySubscription(string userId, bool canAddBulkLicense)
        {
            TempData["UserId"] = userId;
            ViewData["TeamMember"] = userId == null ? string.Empty : LicenseSessionState.Instance.User.Email;
            TempData["CanAddBulk"] = canAddBulkLicense;


            //Logic to get the Subscription details Who are Team Member and Role is assigned as admin by the Super admin
            string adminUserId = string.Empty;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                adminUserId = LicenseSessionState.Instance.User.UserId;
            else
                adminUserId = LicenseSessionState.Instance.AdminId;

            var licenseMapModelList = SubscriLogic.GetSubForLicenseMap(userId, adminUserId);
            return licenseMapModelList;
        }

        public ActionResult LicenseCart(string userId)
        {
            var listdata = GetLicenseListBySubscription(userId, false);
            return View(listdata);
        }

        public ActionResult RevokeLicense(string userId)
        {
            TempData["UserId"] = userId;
            ViewData["TeamMember"] = userLogic.GetUserById(userId).Email;
            List<Models.LicenseMapModel> licenseMapModelList = SubscriLogic.GetUserLicenseDetails(userId, false);
            return View(licenseMapModelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RevokeLicense(params string[] SelectedSubscription)
        {
            UpdateRevokeLicense(SelectedSubscription, "Revoke");
            return RedirectToAction("TeamContainer", "Team");
        }

        public void UpdateRevokeLicense(string[] SelectedSubscription, string action = "Add", string[] SelectedUserIdList = null, bool canAddBulkLicense = false)
        {
            List<string> userIdList = new List<string>();
            if (canAddBulkLicense)
            {
                foreach (var userId in SelectedUserIdList)
                {
                    userIdList.Add(userId);
                }
            }
            else
            {
                userIdList.Add(Convert.ToString(TempData["UserId"]));
            }
            UserLicenseLogic logic = new UserLicenseLogic();
            License.Logic.ServiceLogic.LicenseLogic licenseLogic = new LicenseLogic();
            List<License.Model.UserLicense> userLicesList = new List<License.Model.UserLicense>();
            foreach (var data in SelectedSubscription)
            {
                var splitValue = data.Split(new char[] { '-' });
                var prodId = splitValue[0].Split(new char[] { ':' })[1];
                var subscriptionId = splitValue[1].Split(new char[] { ':' })[1];
                License.Model.UserLicense lic = new License.Model.UserLicense();
                lic.UserId = string.Empty;//Multiple users adding user Id is not required here
                License.Model.LicenseData licData = new License.Model.LicenseData();
                licData.UserSubscriptionId = Convert.ToInt32(subscriptionId);
                licData.ProductId = Convert.ToInt32(prodId);
                lic.License = licData;
                userLicesList.Add(lic);
            }
            if (action == "Add")
                logic.CreateUserLicense(userLicesList, userIdList);
            else
                logic.RevokeUserLicense(userLicesList, userIdList.FirstOrDefault());//temporary data changes
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
            List<License.Model.UserLicenseRequest> licReqList = new List<UserLicenseRequest>();
            foreach (var data in SelectedSubscription)
            {
                var splitValue = data.Split(new char[] { '-' });
                var prodId = splitValue[0].Split(new char[] { ':' })[1];
                var subscriptionId = splitValue[1].Split(new char[] { ':' })[1];

                License.Model.UserLicenseRequest req = new License.Model.UserLicenseRequest();
                req.Requested_UserId = LicenseSessionState.Instance.User.UserId;
                req.ProductId = Convert.ToInt32(prodId);
                req.UserSubscriptionId = Convert.ToInt32(subscriptionId);
                req.RequestedDate = DateTime.Now.Date;
                licReqList.Add(req);
            }
            UserLicenseRequestLogic reqLogic = new UserLicenseRequestLogic();
            reqLogic.Create(licReqList);
            return RedirectToAction("TeamContainer", "Team");
        }
    }
}