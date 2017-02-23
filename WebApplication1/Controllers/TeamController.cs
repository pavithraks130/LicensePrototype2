using License.MetCalWeb.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class TeamController : Controller
    {
        // GET: Team
        public ActionResult TeamContainer()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.Email = "aa@1.com";
            loginViewModel.Password = "123ert";
            loginViewModel.RememberMe = true;
            return View(loginViewModel);
        }

        //public ActionResult TeamMembers()
        //{
        //    return View();
        //}

        //public ActionResult Subscriptions()
        //{
        //    return View();
        //}
    }
}