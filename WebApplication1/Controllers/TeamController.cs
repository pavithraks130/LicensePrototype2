using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.Core.Manager;
using License.Logic.ServiceLogic;
using License.MetCalWeb;
using License.MetCalWeb.Models;
using License.Model.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WebApplication1.Controllers
{
    public class TeamController : Controller
    {
        private TeamMemberLogic logic = null;
        private UserLogic userLogic = null;


        public TeamController()
        {
            logic = new TeamMemberLogic();
            userLogic = new UserLogic();
        }
        // GET: Team
        public ActionResult TeamContainer()
        {
            return View();
        }

        public ActionResult TeamMembers()
        {
            License.Model.Model.UserInviteList inviteList = new UserInviteList();
            string adminId = string.Empty;
            if (LicenseSessionState.Instance.User.Roles.Contains("Admin"))
                adminId = LicenseSessionState.Instance.User.UserId;
            else
                adminId = logic.GetUserAdminDetails(LicenseSessionState.Instance.User.UserId);
            if (!String.IsNullOrEmpty(adminId))
                inviteList = logic.GetUserInviteDetails(adminId);
            return View(inviteList);
        }

        public ActionResult Subscriptions()
        {
            return View();
        }
    }
}