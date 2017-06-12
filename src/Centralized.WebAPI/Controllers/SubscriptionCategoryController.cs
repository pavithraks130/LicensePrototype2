using Centralized.WebAPI.Common;
using LicenseServer.DataModel;
using LicenseServer.Logic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Centralized.WebAPI.Controllers
{
    /// <summary>
    /// Controller for Subscription Category
    /// </summary>
    [Authorize]
    [RoutePrefix("api/SubscriptionCategory")]
    public class SubscriptionCategoryController : BaseController
    {
        private SubscriptionCategoryLogic subscriptionCategoryLogic = null;

        /// <summary>
        /// Constructor for Product Category Controller
        /// </summary>
        public SubscriptionCategoryController()
        {
            subscriptionCategoryLogic = new SubscriptionCategoryLogic();
        }
        /// <summary>
        /// GET Method: Service is used to get the Subscription Categories.
        /// </summary>
        /// <returns>Subscription Category List</returns>
        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetAllSubscriptionCategories()
        {
            return Ok(subscriptionCategoryLogic.GetAll());
        }

        /// <summary>
        /// POST Method: Create Subscription Category
        /// </summary>
        /// <param name="subscriptionCategory">Subscription Category</param>
        /// <returns>Status indicating Subscription Category Creation</returns>
        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage CreateSubscriptionCategory(SubscriptionCategory subscriptionCategory)
        {
            var subscrCategory = subscriptionCategoryLogic.Create(subscriptionCategory);
            if (subscrCategory?.Id > 0)
                return Request.CreateResponse(HttpStatusCode.Created, Constants.Success);
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, subscriptionCategoryLogic.ErrorMessage);
        }

        /// <summary>
        /// GET Method: Get Subscription category based on the id
        /// </summary>
        /// <param name="id">Subscription Category ID</param>
        /// <returns>Subscription Category</returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public HttpResponseMessage GetSubscriptionCategoryById(int id)
        {
            var subscrCategory = subscriptionCategoryLogic.GetById(id);
            if (subscrCategory != null)
                return Request.CreateResponse(HttpStatusCode.OK, subscrCategory);
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, subscriptionCategoryLogic.ErrorMessage);
        }

        /// <summary>
        /// PUT Method: Updating the Category changes based on the categoryId.
        /// </summary>
        /// <param name="id">Subscription Category ID to be updated</param>
        /// <param name="category">Subscription Category Updates</param>
        /// <returns>On success returns updated Subscription Category else returns error</returns>
        [HttpPut]
        [Route("Update/{id}")]
        public HttpResponseMessage UpdateSubscriptionCategory(int id, SubscriptionCategory category)
        {
            var subCategory = subscriptionCategoryLogic.Update(id, category);
            if (subCategory != null && subCategory.Id > 0)
                return Request.CreateResponse(HttpStatusCode.OK, subCategory);
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, subscriptionCategoryLogic.ErrorMessage);
        }

        /// <summary>
        /// Delete Method: Delete the Subscription Category by Id.
        /// </summary>
        /// <param name="id">Subscription Category ID</param>
        /// <returns>Status indicating whether Subscription Category is deleted</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public HttpResponseMessage DeleteSubscriptionCategory(int id)
        {
            bool status = subscriptionCategoryLogic.Delete(id);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, Constants.Deleted);
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, subscriptionCategoryLogic.ErrorMessage);
        }
    }
}
