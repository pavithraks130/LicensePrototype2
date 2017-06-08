using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    public class TeamMemberResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int TeamMemberId { get; set; }
    }
}
