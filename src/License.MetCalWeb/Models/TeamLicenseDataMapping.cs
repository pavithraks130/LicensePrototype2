using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class TeamLicenseDataMapping
    {
        public List<Team> TeamList { get; set; }

        public List<ProductLicense> LicenseDataList { get; set; }
    }
}