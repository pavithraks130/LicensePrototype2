using System;
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
            return View();
        }

        public ActionResult TeamMembers()
        {
            return View();
        }

        public ActionResult Subscriptions()
        {
            return View();
        }
    }
}