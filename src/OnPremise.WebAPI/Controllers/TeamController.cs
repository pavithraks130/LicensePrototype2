using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using License.Logic.DataLogic;
using License.DataModel;
using License.Logic.BusinessLogic;

namespace OnPremise.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/TeamMember")]
    public class TeamController : BaseController
    {
        private TeamMemberLogic logic = null;
        TeamBO teamBoObject = null;
        public TeamController()
        {
            logic = new TeamMemberLogic();
            teamBoObject = new TeamBO();
        }
        public void Initialize()
        {
            logic.UserManager = UserManager;
            logic.RoleManager = RoleManager;
            teamBoObject.UserManager = UserManager;
            teamBoObject.RoleManager = RoleManager;
        }

        [HttpGet]
        [Route("All/{userId}")]
        public IHttpActionResult GetAll(string userId)
        {
            Initialize();
            var userInviteList = teamBoObject.GetUserInviteDetails(userId);
            return Ok(userInviteList);

        }

        [HttpPost]
        [Route("CreateInvite")]
        public HttpResponseMessage CreateInvite(TeamMember member)
        {
            Initialize();
            var teamMemResponseObj = teamBoObject.CreateTeamMembereInvite(member);
            if (teamMemResponseObj != null)
                return Request.CreateResponse(HttpStatusCode.OK, teamMemResponseObj);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, teamBoObject.ErrorMessage);
        }

        [HttpDelete]
        [Route("DeleteInvite/{id}")]
        public HttpResponseMessage DeleteTeamMember(int id)
        {
            var status = logic.DeleteTeamMember(id);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpPut]
        [Route("UpdateInvitation")]
        public HttpResponseMessage UpdateInvitationStatus(TeamMember mem)
        {
            logic.UpdateInviteStatus(mem.Id, mem.InviteeStatus);
            if (string.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.OK, "success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpPut]
        [Route("UpdateAdminAccess")]
        public HttpResponseMessage UpdateAdminAccess(TeamMember mem)
        {
            logic.SetAsAdmin(mem.Id, mem.InviteeUserId, mem.IsAdmin);
            if (string.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.OK, "success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

    }
}
