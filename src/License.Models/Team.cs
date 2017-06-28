using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AdminId { get; set; }
        public User AdminUser { get; set; }
        public bool IsDefaultTeam { get; set; }
        public int ConcurrentUserCount { get; set; }
        public ICollection<TeamMember> TeamMembers { get; set; }
    }
}
