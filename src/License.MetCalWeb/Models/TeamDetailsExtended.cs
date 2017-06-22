using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Models;
namespace License.MetCalWeb.Models
{
    public class TeamDetailsExtended : TeamDetails
    {
        public List<Product> ProductList { get; set; }

        public List<UserLicenseRequest> LicenseRequestList { get; set; }
        public TeamDetailsExtended()
        {
            ProductList = new List<Product>();
        }
    }
}