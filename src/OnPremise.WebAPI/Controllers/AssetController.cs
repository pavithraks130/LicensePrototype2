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

        // Get api/Asset/GetAll
        /// <summary>
        /// Get Method to list all the Hardware assets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAssetes()
        {
            var listAsset = logic.GetAssets();
            return Ok(listAsset);
        }


        // POST api/Asset/CreateAsset
        /// <summary>
        /// Post Method to create the Asset
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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


        // GET api/Asset/GetAssetByID/{id}
        /// <summary>
        /// Get Method to get the asset/Hardware details based on Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetAssetById/{id}")]
        public HttpResponseMessage GetAssetById(int id)
        {
            var asset = logic.GetAssetById(id);
            if (asset != null)
                return Request.CreateResponse(HttpStatusCode.OK, asset);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// Put Method to Update the asset changes based on the Asset Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete Method. Delete the asset by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
