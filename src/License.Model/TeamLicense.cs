using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    public class TeamLicense
    {
        public int Id { get; set; }
        public int LicenseId { get; set; }
        public int TeamId { get; set; }
        public int ProductId { get; set; }
        public bool IsMapped { get; set; }
        public virtual ProductLicense License { get; set; }
    }
   
}
