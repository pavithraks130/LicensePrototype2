using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{

    public class TeamLicenseDataMapping
    {
        public int ConcurrentUserCount { get; set; }
        public List<string> TeamList { get; set; }

        public List<int> ProductIdList { get; set; }
    }

}