using System.Collections.Generic;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;


namespace License.MetCalWeb
{
    public class LicenseSessionState
    {

        public static string UserId { get; set; }
        public static string ServerUserId { get; set; }
        public static bool GlobalAdmin { get; set; }
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

        public bool IsGlobalAdmin
        {
            get { return LicenseSessionState.GlobalAdmin; }
            set
            {
                LicenseSessionState.GlobalAdmin = value;
            }
        }

        public bool IsAdmin { get; set; }

        public bool IsTeamMember { get; set; }

        public AccessToken OnPremiseToken { get; set; }

        public AccessToken CentralizedToken { get; set; }

        public List<Team> TeamList { get; set; }

        /// <summary>
        /// Used in the License and Team manaement modules.
        /// </summary>
        public Team SelectedTeam { get; set; }

        /// <summary>
        /// Used to set the Team Context at global level which will provide the fetaures
        /// which are enables.
        /// </summary>
        public Team AppTeamContext { get; set; }

        public List<SubscriptionDetails> UserSubscriptionList { get; set; }

        public List<dynamic> SubscriptionMonth = new List<dynamic>() {
            new { Value = 1, Name="12 Months" },
            new {Value =2 ,Name="24 Months"},
            new {Value =3 , Name="36 Months" }
        };



    }
}