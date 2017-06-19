using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.MetCalDesktop.Model
{
    public class UserDetails
    {
        public List<Team> Teams { get; set; }
        public List<UserLicense> UserLicenses { get; set; }
    }
}
