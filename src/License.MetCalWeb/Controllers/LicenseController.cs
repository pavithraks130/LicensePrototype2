using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.Logic.Common;
using License.Logic.DataLogic;
using License.MetCalWeb;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using License.DataModel;

namespace License.MetCalWeb.Controllers
{
    public class LicenseController : Controller
    {
        private TeamMemberLogic logic = null;
        private UserLogic userLogic = null;
        private UserLicenseRequestLogic userLicenseRequestLogic = null;

        // GET: License
        public ActionResult Index()
        {
            return View();
        }

        public LicenseController()
        {
            userLicenseRequestLogic = new UserLicenseRequestLogic();
            userLogic = new UserLogic();
            logic = new TeamMemberLogic();

        }

        public ActionResult LicenseApproval()
        {
            List<UserLicenseRequest> requestList = null;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                requestList = userLicenseRequestLogic.GetRequestList(LicenseSessionState.Instance.User.UserId);
            else if (LicenseSessionState.Instance.IsAdmin)
                requestList = userLicenseRequestLogic.GetRequestList(LicenseSessionState.Instance.AdminId);
            return View(requestList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LicenseApproval(string comment, string status, params string[] selectLicenseRequest)
        {
            List<UserLicenseRequest> licReqList = new List<UserLicenseRequest>();
            List<UserLicense> userlicList = new List<UserLicense>();
            foreach (var id in selectLicenseRequest)
            {
                var userlicReq = userLicenseRequestLogic.GetById(Convert.ToInt32(id));
                var licSubId = "ProdId:" + userlicReq.ProductId + "-UserSubId:" + userlicReq.UserSubscriptionId;
                switch (status)
                {
                    case "Approve": userlicReq.IsApproved = true; break;
                    case "Reject": userlicReq.IsRejected = true; break;
                }
                userlicReq.Comment = comment;
                userlicReq.ApprovedBy = LicenseSessionState.Instance.User.UserName;
                licReqList.Add(userlicReq);
                if (status == "Approve")
                {
                    UserLicense lic = new UserLicense();
                    lic.UserId = userlicReq.Requested_UserId;
                    lic.License = new LicenseData();
                    lic.License.ProductId = userlicReq.ProductId;
                    lic.License.UserSubscriptionId = userlicReq.UserSubscriptionId;
                    userlicList.Add(lic);
                }

            }
            if (licReqList.Count > 0)
            {
                userLicenseRequestLogic.Update(licReqList);
                if(userlicList.Count > 0)
                {
                    UserLicenseLogic licLogic = new UserLicenseLogic();
                    licLogic.CreataeUserLicense(userlicList);
                }
            }
            return RedirectToAction("TeamContainer", "Team");
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
            License.DataModel.UserInviteList inviteList = new UserInviteList();
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

            var licenseMapModelList = OnPremiseSubscriptionLogic.GetSubForLicenseMap(userId, adminUserId);
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
            List<Models.LicenseMapModel> licenseMapModelList = OnPremiseSubscriptionLogic.GetUserLicenseDetails(userId, false);
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
            License.Logic.DataLogic.LicenseLogic licenseLogic = new LicenseLogic();
            List<License.DataModel.UserLicense> userLicesList = new List<License.DataModel.UserLicense>();
            foreach (var data in SelectedSubscription)
            {
                var splitValue = data.Split(new char[] { '-' });
                var prodId = splitValue[0].Split(new char[] { ':' })[1];
                var subscriptionId = splitValue[1].Split(new char[] { ':' })[1];
                License.DataModel.UserLicense lic = new License.DataModel.UserLicense();
                lic.UserId = string.Empty;//Multiple users adding user Id is not required here
                License.DataModel.LicenseData licData = new License.DataModel.LicenseData();
                licData.UserSubscriptionId = Convert.ToInt32(subscriptionId);
                licData.ProductId = Convert.ToInt32(prodId);
                lic.License = licData;
                userLicesList.Add(lic);
            }
            if (action == "Add")
                logic.CreateMultiUserLicense(userLicesList, userIdList);
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
            List<License.DataModel.UserLicenseRequest> licReqList = new List<UserLicenseRequest>();
            foreach (var data in SelectedSubscription)
            {
                var splitValue = data.Split(new char[] { '-' });
                var prodId = splitValue[0].Split(new char[] { ':' })[1];
                var subscriptionId = splitValue[1].Split(new char[] { ':' })[1];

                License.DataModel.UserLicenseRequest req = new License.DataModel.UserLicenseRequest();
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

        public ActionResult RequestStatus()
        {
            UserLicenseRequestLogic reqLogic = new UserLicenseRequestLogic();
            var listlic = reqLogic.GetLicenseRequest(LicenseSessionState.Instance.User.UserId);
            return View(listlic);
        }
    }
}