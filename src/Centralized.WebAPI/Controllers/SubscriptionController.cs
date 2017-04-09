using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LicenseServer.Logic;


namespace Centralized.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/subscription")]
    public class SubscriptionController : BaseController
    {

        private SubscriptionTypeLogic logic = null;
        public SubscriptionController()
        {
            logic = new SubscriptionTypeLogic();
        }

        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetAllSubscription()
        {
            var subList = logic.GetSubscriptionType();
            return Ok(subList);
        }


    }
}
