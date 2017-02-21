using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using License.Core.Manager;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace License.WebAPIService.Controller
{
    public class BaseApiController: ApiController
    {

        private readonly AppUserManager _calUserManage = null;
        public AppUserManager UserManager
        {
            get { return _calUserManage ?? Request.GetOwinContext().GetUserManager<AppUserManager>(); }
        }

        private AppRoleManager _calRoleManager = null;
        public AppRoleManager RoleManager
        {
            get { return _calRoleManager ?? Request.GetOwinContext().GetUserManager<AppRoleManager>(); }
        }

        protected HttpResponseMessage GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, "");
            }

            if (!result.Succeeded)
            {
                string error = string.Empty;
                if (result.Errors != null)
                {

                    foreach (string err in result.Errors)
                    {
                        error = error + " " + err;
                        ModelState.AddModelError("", err);
                    }
                }
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, error);
            }

            return null;
        }
    }
}