using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using License.Logic.DataLogic;
using License.Core.Manager;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using Microsoft.Owin.Security;
using License.DataModel;

namespace OnPremise.WebAPI.Common
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId != String.Empty)
                context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userLogic = new UserLogic();
            userLogic.UserManager = context.OwinContext.Get<AppUserManager>();
            userLogic.RoleManager = context.OwinContext.Get<AppRoleManager>();

            var userModel = userLogic.AuthenticateUser(context.UserName, context.Password);
            if (userModel != null)
            {
                var identity = new ClaimsIdentity();
                identity.AddClaim(new Claim("UserId", userModel.UserId));
                identity.AddClaim(new Claim("UserName", userModel.UserName));

                AuthenticationProperties prop = CreateAuthenticationProperties(userModel);
                AuthenticationTicket ticket = new AuthenticationTicket(identity, prop);
                context.Validated(ticket);
            }
            else
            {
                context.SetError("Invalid Credentials");
                return;
            }
        }

        public AuthenticationProperties CreateAuthenticationProperties(User model)
        {
            IDictionary<string, string> propList = new Dictionary<string, string>();
            propList.Add("UserName", model.UserName);
            propList.Add("UserId", model.UserId);
            propList.Add("ServerUserId", model.ServerUserId);
            return new AuthenticationProperties(propList);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var param in context.AdditionalResponseParameters)
            {
                context.AdditionalResponseParameters.Add(param.Key, param.Value);
            }
            return Task.FromResult<Object>(null);
        }
    }
}