using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace License.MetCalWeb.Controllers
{
    public class BaseController : Controller
    {
       
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