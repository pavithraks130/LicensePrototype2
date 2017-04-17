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
    public class TeamMemberController : BaseController
    {
        private TeamMemberLogic logic = null;
        TeamBO teamBoObject = null;
        public TeamMemberController()
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


        /// <summary>
        /// POST Method. To create the User Invite Request to the Team
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
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

        [HttpPost]
        [Route("CreateTeamMember")]
        public HttpResponseMessage CreateTeamMembers(List<TeamMember> teamMemList)
        {
            Initialize();
            var status = teamBoObject.AddTeamMembers(teamMemList);
            return Request.CreateResponse(HttpStatusCode.Created, "Success");
        }

        [HttpPost]
        [Route("RemoveTeamMember")]
        public HttpResponseMessage RemoveTeamMembers(List<TeamMember> teamMemList)
        {
            Initialize();
            var status = teamBoObject.RemoveTeamMembers(teamMemList);
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        /// <summary>
        /// Delete Method. To Remove the Team Member from the Team. but the teama member will be updated to the Default team
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteInvite/{id}")]
        public HttpResponseMessage DeleteTeamMember(int id)
        {
            Initialize();
            var status = logic.DeleteTeamMember(id);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }


        /// <summary>
        /// PUT method. To update the Team Meember for User Invitation Request Status can be updated
        /// </summary>
        /// <param name="mem"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateInvitation")]
        [AllowAnonymous]
        public HttpResponseMessage UpdateInvitationStatus(TeamMember mem)
        {
            Initialize();
            logic.UpdateInviteStatus(mem.Id, mem.InviteeStatus);
            if (string.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.OK, "success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }


        /// <summary>
        /// Put Method. To Update the admin access to the Team Member 
        /// </summary>
        /// <param name="mem"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateAdminAccess")]
        public HttpResponseMessage UpdateAdminAccess(TeamMember mem)
        {
            Initialize();
            logic.SetAsAdmin(mem.Id, mem.InviteeUserId, mem.IsAdmin);
            if (string.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.OK, "success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// Get Method. To fetch all the Team Member details based on the Team Id.
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTeamMemberByTeamId/{teamId}")]
        public HttpResponseMessage GetTeamMemberByUser(int teamId)
        {
            var obj = logic.GetTeamMembers(teamId);
            if (obj != null)
                return Request.CreateResponse(HttpStatusCode.OK, obj);
            else if (obj == null && String.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.OK, "");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// Get Method. To get the User Invitation / Team Member record based on the UserId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTeamMemberByUserId/{userId}")]
        public HttpResponseMessage GetTeamMemberByUser(string userId)
        {
            var obj = logic.GetTeamMemberByUserId(userId);
            if (obj != null)
                return Request.CreateResponse(HttpStatusCode.OK, obj);
            else if (obj == null && String.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.OK, "");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }
    }
}
