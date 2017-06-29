using Centralized.WebAPI.Common;
using License.Models;
using LicenseServer.Logic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Centralized.WebAPI.Controllers
{
    /// <summary>
    /// Controller for Feature Related Operations
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Feature")]
    public class FeatureController : BaseController
    {
        private FeaturesLogic featurelogic = null;

        /// <summary>
        /// Constructor for Feature Controller 
        /// </summary>
        public FeatureController()
        {
            featurelogic = new FeaturesLogic();
        }

        /// <summary>
        /// Get All the Feature List from Db
        /// </summary>
        /// <returns>List of Features</returns>
        [Route("All")]
        [HttpGet]
        public IHttpActionResult GetAllFeatures()
        {
            return Ok(featurelogic.GetFeatureList());
        }

        /// <summary>
        /// Gets Feature By ID
        /// </summary>
        /// <param name="id">Feature ID</param>
        /// <returns>On successfully finding Feature, returns Feature else returns error</returns>
        [Route("GetById/{id}")]
        [HttpGet]
        public HttpResponseMessage GetFeatureById(int id)
        {
            var data = featurelogic.GetFeatureById(id);
            if (data != null)
                return Request.CreateResponse(HttpStatusCode.OK, data);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, featurelogic.ErrorMessage);
        }

        /// <summary>
        /// Create Feature Record in database
        /// </summary>
        /// <param name="feature">Feature to be created</param>
        /// <returns>Status of Feature Creation</returns>
        [Route("Create")]
        [HttpPost]
        public HttpResponseMessage CreateFeature(Feature feature)
        {
            feature = featurelogic.CreateFeature(feature);
            if (feature?.Id > 0)
                return Request.CreateResponse(HttpStatusCode.Created, feature);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, featurelogic.ErrorMessage);
        }

        /// <summary>
        /// Update Existing Feature with Changes.
        /// </summary>
        /// <param name="id">Feature ID</param>
        /// <param name="featureToUpdate">Feature to Update</param>
        /// <returns>Status indicating if Feature is Updated</returns>
        [Route("Update/{id}")]
        [HttpPut]
        public HttpResponseMessage UpdateFeature(int id, Feature featureToUpdate)
        {
            var feature = featurelogic.Update(id, featureToUpdate);
            if (feature?.Id > 0)
                return Request.CreateResponse(HttpStatusCode.OK, feature);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, featurelogic.ErrorMessage);
        }

        /// <summary>
        /// Delete Feature with Specific ID
        /// </summary>
        /// <param name="id">Feature ID</param>
        /// <returns>Status indicating if Feature is Deleted</returns>
        [Route("Delete/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteFeature(int id)
        {
            var feature = featurelogic.DeleteFeature(id);
            if (feature != null && string.IsNullOrEmpty(featurelogic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.OK, feature);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, featurelogic.ErrorMessage);
        }

        /// <summary>
        /// Get Features by Category
        /// </summary>
        /// <param name="categoryId">Feature Category ID</param>
        /// <returns>Feature List By Category</returns>
        [HttpGet]
        [Route("GetByCategory/{categoryId}")]
        public IHttpActionResult GetFeaturesByCategory(int categoryId)
        {
            var featureList = featurelogic.GetFeatureByCategoryId(categoryId);
            return Ok(featureList);
        }


    }
}