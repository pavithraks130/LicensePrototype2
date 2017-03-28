using System.Collections.Generic;


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

        public Models.UserModel User { get; set; }

        public bool IsAuthenticated { get; set; }

        public bool IsSuperAdmin { get; set; }

        public bool IsAdmin { get; set; }

		public bool IsGlobalAdmin { get; set; }

		public IEnumerable<Models.SubscriptionProductModel> SubscriptionList { get; set; }

        public List<Models.LicenseMapModel>  ProductLicense { get; set; }

    }
}