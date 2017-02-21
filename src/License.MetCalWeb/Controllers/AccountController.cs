using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace License.MetCalWeb.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            return View();

        }

        public ActionResult LogIn()
        {
            return View();
        }


    }
}