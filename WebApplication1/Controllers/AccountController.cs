using Microsoft.Owin.Security;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using License.Core.Manager;
using License.Logic.ServiceLogic;
using License.MetCalWeb.Models;
using License.Model.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WebApplication1.Controllers
{
    public class AccountController : BaseController
    {
        private UserLogic logic = new UserLogic();

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (logic.UserManager == null)
                    logic.UserManager = Request.GetOwinContext().GetUserManager<AppUserManager>();
                User user = logic.AutheticateUser(model.Email, model.Password);
                if (user != null)
                {
                    SignInAsync(user, model.RememberMe);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("","invalid Credentials");
                }
            }
            return View();
        }

        private void  SignInAsync(User user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = logic.CreateIdentity(user);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }
    }
}