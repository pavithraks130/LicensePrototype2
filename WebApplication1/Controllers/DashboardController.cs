using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.Core.Manager;

using License.Core.Model;
using License.Logic.Common;
using License.Logic.ServiceLogic;
using License.MetCalWeb;
using License.MetCalWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using License.MetCalWeb.Common;
using TeamMembers = License.Model.Model.TeamMembers;

namespace License.MetCalWeb.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private TeamMemberLogic logic = null;
        private UserLogic userLogic = null;

        public DashboardController()
        {
            logic = new TeamMemberLogic();
            userLogic = new UserLogic();
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

    }
}