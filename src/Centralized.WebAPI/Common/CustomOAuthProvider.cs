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
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace Centralized.WebAPI.Common
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }


        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin",new[] { "http://"+context.Request.Host.Value });
            UserLogic logic = new UserLogic();
            logic.UserManager = context.OwinContext.GetUserManager<LicUserManager>();
            logic.RoleManager = context.OwinContext.GetUserManager<LicRoleManager>();
            User user = logic.AuthenticateUser(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("", "invalid grant");
                return;
            }

            var identity = logic.CreateClaimsIdentity(user.UserId, context.Options.AuthenticationType);
            //var identity1 = logic.CreateClaimsIdentity(user.UserId, CookieAuthenticationDefaults.AuthenticationType);
            AuthenticationProperties properties = CreateProperties(user);


            var authTicket = new AuthenticationTicket(identity, properties);
            context.Validated(authTicket);
            // context.Request.Context.Authentication.SignIn(identity1);

        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(User userObj)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
              {
                  { "userName", userObj.UserName},
                  { "Id",userObj.UserId.ToString()}

              };
            return new AuthenticationProperties(data);
        }
    }
}