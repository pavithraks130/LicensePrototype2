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
            string clientId = string.Empty, clientSecret = string.Empty;
            context.TryGetBasicCredentials(out clientId, out clientSecret);
            if (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecret) && ValidateEncryptedApplicationIdentity(clientId, clientSecret))
            {
                context.Validated();
            }
            else
            {
                context.SetError("invalid_client", "Client credentials could not be retrieved through the Authorization header.");
                context.Rejected();
            }
        }

        private bool ValidateEncryptedApplicationIdentity(string clientId, string clientSecret)
        {
            var decryptedApplicationIdentity = LicenseKey.EncryptDecrypt.DecryptString(clientId, clientSecret);
            //TODO: Need this to be more robust, right now product IDS are being set in a range for a single product
            return decryptedApplicationIdentity.Contains("TO");
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
                {"ServerUserId",String.IsNullOrEmpty(userObj.ServerUserId)?"0":userObj.ServerUserId }

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