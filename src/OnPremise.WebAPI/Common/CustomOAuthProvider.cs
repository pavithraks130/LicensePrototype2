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
            if (userModel == null)
            {
                context.SetError("Invalid Credentials");
                return;
            }
            else
            {
                var identity = userLogic.CreateClaimsIdentity(userModel.UserId, context.Options.AuthenticationType);
                identity.AddClaim(new Claim("UserId", userModel.UserId));
                identity.AddClaim(new Claim("UserName", userModel.UserName));

                AuthenticationProperties prop = CreateAuthenticationProperties(userModel);
                AuthenticationTicket ticket = new AuthenticationTicket(identity, prop);
                context.Validated(ticket);
            }
        }

        public AuthenticationProperties CreateAuthenticationProperties(User userObj)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
              {
                  { "userName", userObj.UserName},
                  { "Id",userObj.UserId.ToString()},
                {"ServerUserId",userObj.ServerUserId }

              };

            return new AuthenticationProperties(data);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<Object>(null);
        }
    }
}