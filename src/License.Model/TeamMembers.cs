using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace License.DataModel
{
       public class TeamMember
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string InviteeEmail { get; set; }
        public string InviteeUserId { get; set; }
        public string InviteeStatus { get; set; }
        public DateTime InvitationDate { get; set; }
        public User InviteeUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSelected { get; set; }
        public bool IsActive { get; set; }
        [JsonIgnore]
        public Team Team { get; set; }
    }

}
