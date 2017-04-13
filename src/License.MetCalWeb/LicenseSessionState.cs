using System.Collections.Generic;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;


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

        public bool IsGlobalAdmin { get; set; }

        public bool IsTeamMember { get; set; }

        public AccessToken OnPremiseToken { get; set; }

        public AccessToken CentralizedToken { get; set; }

        public TeamMember TeamMeberDetails { get; set; }

        public List<Team> TeamList { get; set; }

        public Team SelectedTeam { get; set; }
       
    }
}