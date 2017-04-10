using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace License.Core.Model
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AdminId { get; set; }
        [ForeignKey("AdminId")]
        public virtual AppUser AdminUser { get; set; }
        public  ICollection<TeamMember> TeamMembers { get; set; }

    }
}
