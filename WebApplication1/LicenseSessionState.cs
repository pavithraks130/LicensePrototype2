using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Core.Manager;
using License.Core.Model;
using License.Model;

namespace License.MetCalWeb
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

        public bool IsAuthenticated { get; set; }

        public bool IsSuperAdmin { get; set; }

        public bool IsAdmin { get; set; }

        public List<MetCalWeb.Models.LicenseMapModel> LicenseMapModelList { get; set; }
        public IEnumerable<License.MetCalWeb.Models.SubscriptionProductModel> SubscriptionList { get; set; }

    }
}