using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using License.DataModel;
using License.Logic.DataLogic;
using License.Logic.BusinessLogic;

namespace OnPremise.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/TeamLicense")]
    public class TeamLicenseController : BaseController
    {
        UserLicenseLogic userLicenselogic = null;
        TeamLicenseLogic teamLicenselogic = null;
        UserLicenseRequestLogic reqLogic = null;

        public TeamLicenseController()
        {
            userLicenselogic = new UserLicenseLogic();
            teamLicenselogic = new TeamLicenseLogic();
            reqLogic = new UserLicenseRequestLogic();
        }
     
        /// <summary>
        /// Post method. Map the License to the Team. The license will be fetched based on the subscription and Product Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage AddTeamLicense(TeamLicenseDataMapping model)
        {
            var status = teamLicenselogic.CreateTeamLicense(model);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, userLicenselogic.ErrorMessage);
        }

        /// <summary>
        /// Delete Method : To delete the team License
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Revoke")]
        public HttpResponseMessage DeleteTeamLicenses(TeamLicenseDataMapping data)
        {
            TeamBO teamBOLogic = new TeamBO();
            var status = teamBOLogic.DeleteTeamLicense(data);
            if (status)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, teamBOLogic.ErrorMessage);
        }

        /// <summary>
        /// Post Method. Used to fetch the Team Subscribed License List based on the Team Id .
        /// Features will be fetched based on the user Requirement. The input to the service is FetchUserSubscription
        /// which contains UserId, TeamId and IsFeatureRequired Property.
        /// This request can be used to fetch the User License details based on the Teama Id .
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTeamLicenseByTeam/{teamId}")]
        public HttpResponseMessage GetTeamSubscritionLicense(int teamId)
        {
            TeamBO teamBOLogic = new TeamBO();
            var data = teamBOLogic.GetTeamLicenseProductByTeamId(teamId);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
    }
}
