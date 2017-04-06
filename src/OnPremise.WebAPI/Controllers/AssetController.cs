using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using License.DataModel;
using License.Logic.DataLogic;

namespace OnPremise.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Asset")]
    public class AssetController : BaseController
    {
        TeamAssetLogic logic = null;
        public AssetController()
        {
            logic = new TeamAssetLogic();
        }

        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAssetes()
        {
            var listAsset = logic.GetAssets();
            return Ok(listAsset);
        }

        [HttpPost]
        [Route("CreateAsset")]
        public HttpResponseMessage CreateAsset(TeamAsset model)
        {
            var asset = logic.CreateAsset(model);
            if (asset != null)
                return Request.CreateResponse(HttpStatusCode.OK, asset);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpPut]
        [Route("UpdateAsset/{id}")]
        public HttpResponseMessage UpdateAsset(int id, TeamAsset model)
        {
            var asset = logic.UpdateAsset(id, model);
            if (asset != null)
                return Request.CreateResponse(HttpStatusCode.OK, asset);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpDelete]
        [Route("DeleteAsset/{id}")]
        public HttpResponseMessage DeleteAsset(int id)
        {
            var status = logic.RemoveAsset(id);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }
    }
}
