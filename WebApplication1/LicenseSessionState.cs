using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Core.Manager;
using License.Core.Model;
using License.Model.Model;

namespace WebApplication1
{
    public class LicenseSessionState
    {

        public static LicenseSessionState Instance
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["LicenseWebInstance"] == null)
                    System.Web.HttpContext.Current.Session["LicenseWebInstance"] = new LicenseSessionState();
                return System.Web.HttpContext.Current.Session["LicenseWebInstance"] as LicenseSessionState;
            }
        }

        public User User { get; set; }
       
    }
}