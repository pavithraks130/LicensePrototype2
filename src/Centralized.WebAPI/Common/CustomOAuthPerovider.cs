using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using LicenseServer.Logic;
using Owin;
using Microsoft.Owin;
using LicenseServer.DataModel;
using LicenseServer.Core.Manager;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;

namespace Centralized.WebAPI.Common
{
    public class CustomOAuthPerovider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            UserLogic logic = new UserLogic();
            logic.UserManager = context.OwinContext.GetUserManager<LicUserManager>();
            logic.RoleManager = context.OwinContext.GetUserManager<LicRoleManager>();
            User user = logic.AuthenticateUser(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("", "invalid grant");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("username", user.UserName));
            identity.AddClaim(new Claim("userId", user.UserId));
            identity.AddClaim(new Claim("Role", user.Roles.FirstOrDefault()));

            context.Validated(identity);
        }
    }
}