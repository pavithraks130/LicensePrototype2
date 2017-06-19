using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.MetCalDesktop.Model
{
    public class UserLicense
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int LicenseId { get; set; }

        public int TeamId { get; set; }

        public bool IsTeamLicense { get; set; }

        public int TeamLicenseId { get; set; }

        public ProductLicense License { get; set; }
    }

    public class ProductLicense
    {
        public int Id { get; set; }

        public string AdminUserId { get; set; }

        public string LicenseKey { get; set; }

        public int ProductId { get; set; }
        
    }
}
