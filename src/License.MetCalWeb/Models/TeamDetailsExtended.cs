using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Models;
namespace License.MetCalWeb.Models
{
    public class TeamDetailsExtended:TeamDetails
    {
        //public TeamDetails TeamDetails { get; set; }
        //public Team Team { get { return TeamDetails.Team; } set { TeamDetails.Team = value; } }
        //public List<TeamMember> PendinigUsers { get { return TeamDetails.PendinigUsers; } set { TeamDetails.PendinigUsers = value; } }
        //public List<TeamMember> AcceptedUsers { get { return TeamDetails.AcceptedUsers; } set { TeamDetails.AcceptedUsers = value; } }

        public List<Product> ProductList { get; set; }
        public List<UserLicenseRequest> LicenseRequestList { get; set; }
        public TeamDetailsExtended()
        {
            ProductList = new List<Product>();
            LicenseRequestList = new List<UserLicenseRequest>();
        }
        public TeamDetailsExtended(TeamDetails dtls)
        {
            ProductList = new List<Product>();
            LicenseRequestList = new List<UserLicenseRequest>();

            Team = dtls.Team;
            PendinigUsers = dtls.PendinigUsers;
            AcceptedUsers = dtls.AcceptedUsers;
        }
    }
}