using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    public class UserLicenseDetails
    {
        public User User { get; set; }
        public List<Product> Products { get; set; }
    }
}
