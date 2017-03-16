using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace License.MetCalWeb.Controllers
{
    public class BaseController : Controller
    {
        //private AppUserManager _userManager = null;
        //public AppUserManager UserManager
        //{
        //    get
        //    {
        //        if (_userManager == null)
        //            _userManager = Request.GetOwinContext().GetUserManager<AppUserManager>();
        //        return _userManager;
        //    }
        //}

        //private AppRoleManager _roleManager = null;
        //public AppRoleManager RoleManager
        //{
        //    get
        //    {
        //        if (_roleManager == null)
        //            _roleManager = Request.GetOwinContext().GetUserManager<AppRoleManager>();
        //        return _roleManager;
        //    }
        //}
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            RedirectToAction("Login", "Account");
        }
        public void GetErrorResult(List<string> result)
        {
            foreach (string error in result)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}