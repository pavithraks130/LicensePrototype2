using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LicenseServer.Logic;
using LicenseServer.DataModel;

namespace Centralized.WebAPI.Controllers
{
    public class UserTokenController : BaseController
    {
        private UserTokenLogic logic = null;

        public UserTokenController()
        {
            logic = new UserTokenLogic();
        }

        public IHttpActionResult GetAll()
        {
            var listToken = logic.GetUsertokenList();
            return Ok(listToken);
        }
    }
}
