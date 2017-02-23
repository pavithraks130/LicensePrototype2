using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Model.Model
{
    public class UserInvite
    {
        public int Id { get; set; }
        public string AdminId { get; set; }
        public int TeamId { get; set; }
        public string InviteeEmail { get; set; }
        public string InviteeUserId { get; set; }
        public string InviteeStatus { get; set; }
        public DateTime InvitationDate { get; set; }
        public User AdminUser { get; set; }
        public User InviteeUser { get; set; }
    }
}
