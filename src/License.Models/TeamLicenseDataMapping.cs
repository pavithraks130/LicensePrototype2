using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Models
{
    public class TeamLicenseDataMapping
    {
        public string AdminID { get; set; }
        public List<Team> TeamList { get; set; }
        public List<ProductLicense> LicenseDataList { get; set; }
    }
}
