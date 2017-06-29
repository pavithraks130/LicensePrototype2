using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using License.Models;
using License.ServiceInvoke;

namespace License.MetCalWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {


        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            //AccountLogic _accLogic = new AccountLogic();
            //if (LicenseSessionState.GlobalAdmin)
            //    _accLogic.UpdateLogoutStatus(LicenseSessionState.UserId, ServiceType.CentralizeWebApi);
            //else
            //    _accLogic.UpdateLogoutStatus(LicenseSessionState.UserId, ServiceType.OnPremiseWebApi);
            //if (!String.IsNullOrEmpty(LicenseSessionState.ServerUserId))
            //    _accLogic.UpdateLogoutStatus(LicenseSessionState.ServerUserId, ServiceType.CentralizeWebApi);
           // this.Response.Redirect("Account/Login");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //AccountLogic _accLogic = new AccountLogic();
            //if (LicenseSessionState.GlobalAdmin)
            //    _accLogic.UpdateLogoutStatus(LicenseSessionState.UserId, ServiceType.CentralizeWebApi);
            //else
            //    _accLogic.UpdateLogoutStatus(LicenseSessionState.UserId, ServiceType.OnPremiseWebApi);
            //if (!String.IsNullOrEmpty(LicenseSessionState.ServerUserId))
            //    _accLogic.UpdateLogoutStatus(LicenseSessionState.ServerUserId, ServiceType.CentralizeWebApi);
        }
    }
}
