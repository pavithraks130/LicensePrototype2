using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.Core.Manager;
using License.Logic.ServiceLogic;
using License.MetCalWeb;
using License.MetCalWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WebApplication1.Controllers
{
    public class TeamController : Controller
    {
        private UserInviteLogic logic = null;
        private UserLogic userLogic = null;


        public TeamController()
        {
            logic = new UserInviteLogic();
            userLogic = new UserLogic();
        }
        // GET: Team
        public ActionResult TeamContainer()
        {
            return View();
        }

        public ActionResult TeamMembers()
        {
            License.Model.Model.UserInviteList inviteList = null;
            string adminId = string.Empty;
            if (LicenseSessionState.Instance.User.Roles.Contains("Admin"))
                adminId = LicenseSessionState.Instance.User.UserId;
            else
                adminId = logic.GetUserAdminDetails(LicenseSessionState.Instance.User.UserId);

            inviteList = logic.GetUserInviteDetails(adminId);
            TeamModel model = new TeamModel();
            model.PendinigUsers = inviteList.PendingInvites;
            model.AcceptedUsers = inviteList.AcceptedInvites;
            return View(inviteList);
        }

        public ActionResult Subscriptions()
        {
            return View();
        }
    }
}