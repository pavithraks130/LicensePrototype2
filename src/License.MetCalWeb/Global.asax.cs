using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using License.MetCalWeb.Logic;

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
            AccountLogic _accLogic = new AccountLogic();
            if (LicenseSessionState.GlobalAdmin)
                _accLogic.UpdateLogoutStatus(LicenseSessionState.UserId, Common.ServiceType.CentralizeWebApi);
            else
                _accLogic.UpdateLogoutStatus(LicenseSessionState.UserId, Common.ServiceType.OnPremiseWebApi);
            if (!String.IsNullOrEmpty(LicenseSessionState.ServerUserId))
                _accLogic.UpdateLogoutStatus(LicenseSessionState.ServerUserId, Common.ServiceType.CentralizeWebApi);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            AccountLogic _accLogic = new AccountLogic();
            if (LicenseSessionState.GlobalAdmin)
                _accLogic.UpdateLogoutStatus(LicenseSessionState.UserId, Common.ServiceType.CentralizeWebApi);
            else
                _accLogic.UpdateLogoutStatus(LicenseSessionState.UserId, Common.ServiceType.OnPremiseWebApi);
            if (!String.IsNullOrEmpty(LicenseSessionState.ServerUserId))
                _accLogic.UpdateLogoutStatus(LicenseSessionState.ServerUserId, Common.ServiceType.CentralizeWebApi);
        }
    }
}
