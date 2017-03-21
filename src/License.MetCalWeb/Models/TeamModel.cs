using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Model;

namespace License.MetCalWeb.Models
{
    public class TeamModel
    {
        public User AdminUser { get; set; }
        public List<TeamMembers> PendinigUsers { get; set; }
        public List<TeamMembers> AcceptedUsers { get; set; }
    }
}