using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using License.Core.Manager;
using License.Core.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace WebApplication1.Provider
{
    public class AutorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override  Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var userManager = context.OwinContext.GetUserManager<AppUserManager>();
            var user = userManager.Find(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");
            oAuthIdentity.AddClaim(new Claim("UserId", user.Id));
            var ticket = new AuthenticationTicket(oAuthIdentity, null);
            context.Validated(ticket);
        }
    }
}