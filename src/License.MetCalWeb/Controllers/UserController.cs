using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.Logic.ServiceLogic;
using License.MetCalWeb.Models;

namespace License.MetCalWeb.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private UserLogic logic = new UserLogic();
        LicenseServer.Logic.UserLogic userLogic = new LicenseServer.Logic.UserLogic();
        // GET: User
        public ActionResult Index()
        {
            var usersList = userLogic.GetUsers().ToList();
            List<UserModel> users = new List<UserModel>();
            foreach (var user in usersList)
            {
                if (user.Email == LicenseSessionState.Instance.User.Email)
                    continue;
                UserModel model = new UserModel();
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Name = user.Name;
                model.Email = user.Email;
                model.Organization.Name = user.Organization.Name;
                model.IsActive = user.IsActive;
                foreach (var obj in user.SubscriptionList)
                {
                    model.SubscriptionList.Add(new SubscriptionType() { Name = obj.Name });
                }
                users.Add(model);
            }
            return View(users);
        }


        public ActionResult Profile()
        {
            var user = LicenseSessionState.Instance.User;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(UserModel usermodel, string userId)
        {
            if (ModelState.IsValid)
            {
                bool status = false;
                if (LicenseSessionState.Instance.IsGlobalAdmin)
                {
                    LicenseServer.DataModel.User user = new LicenseServer.DataModel.User();
                    user.FirstName = usermodel.FirstName;
                    user.LastName = usermodel.LastName;
                    user.PhoneNumber = usermodel.PhoneNumber;
                    status = userLogic.UpdateUser(userId, user);
                }
                else if (LicenseSessionState.Instance.IsSuperAdmin)
                {
                    LicenseServer.DataModel.User userObj = new LicenseServer.DataModel.User();
                    userObj.FirstName = usermodel.FirstName;
                    userObj.LastName = usermodel.LastName;
                    userObj.PhoneNumber = usermodel.PhoneNumber;
                    status = userLogic.UpdateUser(LicenseSessionState.Instance.User.ServerUserId, userObj);

                    License.Model.User user = new License.Model.User();
                    user.FirstName = usermodel.FirstName;
                    user.LastName = usermodel.LastName;
                    user.PhoneNumber = usermodel.PhoneNumber;
                    status = logic.UpdateUser(userId, user);
                }
                else
                {

                    License.Model.User user = new License.Model.User();
                    user.FirstName = usermodel.FirstName;
                    user.LastName = usermodel.LastName;
                    user.PhoneNumber = usermodel.PhoneNumber;
                    status = logic.UpdateUser(userId, user);
                }
                LicenseSessionState.Instance.User.FirstName = usermodel.FirstName;
                LicenseSessionState.Instance.User.LastName = usermodel.LastName;
                LicenseSessionState.Instance.User.Name = usermodel.FirstName + " " + usermodel.LastName;
                LicenseSessionState.Instance.User.PhoneNumber = usermodel.PhoneNumber;
                if (status)
                    return RedirectToAction("Home", "Tab");
                ModelState.AddModelError("", logic.ErrorMessage);
            }
            return View(usermodel);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            var changePwdModel = new Models.ChangePassword();
            return View(changePwdModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                string userId = LicenseSessionState.Instance.User.UserId;
                bool status = false;
                if (LicenseSessionState.Instance.IsGlobalAdmin)
                    status = userLogic.ChangePassword(userId, model.CurrentPassword, model.NewPassword);
                else if (LicenseSessionState.Instance.IsSuperAdmin)
                {
                    string serverUserId = LicenseSessionState.Instance.User.ServerUserId;
                    status = userLogic.ChangePassword(serverUserId, model.CurrentPassword, model.NewPassword);
                    status = logic.ChangePassword(userId, model.CurrentPassword, model.NewPassword);
                }
                else
                    status = logic.ChangePassword(userId, model.CurrentPassword, model.NewPassword);

                if (status)
                    return RedirectToAction("Home", "Tab");
                ModelState.AddModelError("", logic.ErrorMessage);
            }
            return View(model);
        }
    }
}