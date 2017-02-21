using System.Web;
using System.Web.Mvc;
using License.Core.Manager;
using License.Logic.ServiceLogic;
using License.MetCalWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WebApplication1.Controllers
{
    public class AccountController : BaseController
    {
        private UserLogic logic = new UserLogic();


        public ActionResult Register()
        {
            ViewData["SucessMessageDisplay"] = false;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            ViewData["SucessMessageDisplay"] = false;
            if (ModelState.IsValid)
            {
                if (logic.UserManager == null)
                    logic.UserManager = Request.GetOwinContext().GetUserManager<AppUserManager>();
                if (logic.RoleManager == null)
                    logic.RoleManager = Request.GetOwinContext().GetUserManager<AppRoleManager>();
                IdentityResult result = logic.CreateUser(model.RegistratoinModel);
                if (result.Succeeded)
                    ViewData["SucessMessageDisplay"] = true;
                else
                    GetErrorResult(result);
            }
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }
    }
}