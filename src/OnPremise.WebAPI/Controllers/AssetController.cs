using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using License.Models;
using License.Logic.DataLogic;

namespace OnPremise.WebAPI.Controllers
{
    /// <summary>
    /// Asset Service 
    /// </summary>
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
        [Route("All")]
        public IHttpActionResult Get()
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
        [Route("Create")]
        public HttpResponseMessage Create(TeamAsset model)
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
        [Route("GetById/{id}")]
        public HttpResponseMessage GetById(int id)
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
        [Route("Update/{id}")]
        public HttpResponseMessage Update(int id, TeamAsset model)
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
        [Route("Delete/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var status = logic.RemoveAsset(id);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }
    }
}
