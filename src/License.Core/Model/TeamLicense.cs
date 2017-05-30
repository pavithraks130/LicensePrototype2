using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Core.Model
{
    public class TeamLicense
    {
        [Key]
        public int Id { get; set; }
        public int LicenseId { get; set; }
        public int TeamId { get; set; }
        public bool IsMapped { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("LicenseId")]
        public virtual LicenseData License { get; set; }
    }
}
