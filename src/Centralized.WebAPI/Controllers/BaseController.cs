using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using LicenseServer.Core.Manager;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace Centralized.WebAPI.Controllers
{
    public class BaseController : ApiController
    {
        private LicUserManager _userManager = null;
        public LicUserManager UserManager
        {
            get { return _userManager ?? (_userManager = Request.GetOwinContext().GetUserManager<LicUserManager>()); }
        }

        private LicRoleManager _roleManager = null;
        public LicRoleManager RoleManager
        {
            get { return _roleManager ?? (_roleManager = Request.GetOwinContext().GetUserManager<LicRoleManager>()); }
        }
    }
}