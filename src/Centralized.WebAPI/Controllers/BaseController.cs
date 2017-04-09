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
            get
            {
                if (_userManager == null)
                    _userManager = HttpContext.Current.GetOwinContext().Get<LicUserManager>();
                return _userManager;
            }
        }

        private LicRoleManager _roleManager = null;
        public LicRoleManager RoleManager
        {
            get
            {
                if (_roleManager == null)
                    _roleManager = Request.GetOwinContext().GetUserManager<LicRoleManager>();
                return _roleManager;
            }
        }

    }
}