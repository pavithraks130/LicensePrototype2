using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class TeamDetails
    {
        public Team Team { get; set; }
        public List<TeamMember> PendinigUsers { get; set; }
        public List<TeamMember> AcceptedUsers { get; set; }

        public List<ProductDetails> ProductList { get; set; }

        public List<UserLicenseRequest> LicenseRequestList { get; set; }
        public TeamDetails()
        {
            ProductList = new List<ProductDetails>();
        }
    }
}