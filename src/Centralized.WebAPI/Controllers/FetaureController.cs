using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LicenseServer.Logic;
using LicenseServer.DataModel;

namespace Centralized.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Feature")]
    public class FetaureController : BaseController
    {
        private FeaturesLogic logic = null;
        public FetaureController()
        {
            logic = new FeaturesLogic();
        }

        /// <summary>
        /// Get All the Feature List from Db
        /// </summary>
        /// <returns></returns>
        [Route("All")]
        [HttpGet]
        public IHttpActionResult GetAllFeatures()
        {
            return Ok(logic.GetFeatureList());
        }

        [Route("GetById/{id}")]
        [HttpGet]
        public HttpResponseMessage GetFeatureById(int id)
        {
            var data = logic.GetFeatureById(id);
            if (data != null)
                return Request.CreateResponse(HttpStatusCode.OK, data);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// Create Feature Record in database
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost]
        public HttpResponseMessage CreataeFeature(Feature f)
        {
            bool status = logic.CreateFeature(f);
            if (status)
                return Request.CreateResponse(HttpStatusCode.Created, "success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// Update Existing Record  with Changes.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        [Route("Update/{id}")]
        [HttpPut]
        public HttpResponseMessage UpdateFeature(int id, Feature f)
        {
            bool status = logic.Update(id, f);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// Delete spsecified Id Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Delete/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteFeature(int id)
        {
            bool status = logic.DeleteFeature(id);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpGet]
        [Route("GetByCategory/{categoryId}")]
        public IHttpActionResult FeaturesByCategory(int categoryId)
        {
            var featureList = logic.GetFeatureByCategoryId(categoryId);
            return Ok(featureList);
        }


    }
}