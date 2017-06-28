using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using LicenseServer.Logic;
using Owin;
using Microsoft.Owin;
using License.Models;
using LicenseServer.Core.Manager;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace Centralized.WebAPI.Common
{
    /// <summary>
    /// Custom Auth Provider
    /// </summary>
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// Validates Token Authentication
        /// </summary>
        /// <param name="context">Authentication Context</param>
        /// <returns>Status indicating Client Authenticity</returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }


        }

        /// <summary>
        /// Grants Resource Owner Credentials
        /// </summary>
        /// <param name="context">Credentials Context</param>
        /// <returns>Resource Owner Credentials if Successful</returns>
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

        /// <summary>
        /// Adds Additional Response Parameters
        /// </summary>
        /// <param name="context">Token Endpoint Context</param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Creates Properties
        /// </summary>
        /// <param name="userObj">User Object</param>
        /// <returns>Authentication Properties</returns>
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