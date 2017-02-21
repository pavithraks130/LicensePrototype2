using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using License.Core.Manager;
using License.Logic.ServiceLogic;
using License.MetCalWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace License.MetCalWeb.Controllers
{
    public class AccountController : Controller
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
            if (ModelState.IsValid)
            {
                if (logic.UserManager == null)
                    logic.UserManager = Request.GetOwinContext().GetUserManager<AppUserManager>();
                if (logic.RoleManager == null)
                    logic.RoleManager = Request.GetOwinContext().GetUserManager<AppRoleManager>();
                IdentityResult result = logic.CreateUser(model.RegistratoinModel);
               return View("LogIn");
            }
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }


    }
}