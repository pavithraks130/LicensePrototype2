using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using License.Core.Manager;
using Microsoft.AspNet.Identity.Owin;

namespace License.MetCalWeb.Controllers
{
    public class BaseController : Controller
    {
        private AppUserManager _userManager = null;
        public AppUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                    _userManager = Request.GetOwinContext().GetUserManager<AppUserManager>();
                return _userManager;
            }
        }

        private AppRoleManager _roleManager = null;
        public AppRoleManager RoleManager
        {
            get
            {
                if (_roleManager == null)
                    _roleManager = Request.GetOwinContext().GetUserManager<AppRoleManager>();
                return _roleManager;
            }
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            RedirectToAction("Login", "Account");
        }
        public void GetErrorResult(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
        }
    }
}