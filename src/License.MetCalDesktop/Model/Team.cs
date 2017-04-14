using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.MetCalDesktop.Model
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AdminId { get; set; }
        public User AdminUser { get; set; }
        public bool IsDefaultTeam { get; set; }
    }
}
