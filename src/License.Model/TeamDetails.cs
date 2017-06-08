using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    public class TeamDetails
    {
        public Team Team { get; set; }
        public List<TeamMember> PendinigUsers { get; set; }
        public List<TeamMember> AcceptedUsers { get; set; }
        public TeamDetails()
        {
            PendinigUsers = new List<TeamMember>();
            AcceptedUsers = new List<TeamMember>();
        }
    }
}
