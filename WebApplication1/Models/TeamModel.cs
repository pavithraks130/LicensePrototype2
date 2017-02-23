using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Model.Model;

namespace License.MetCalWeb.Models
{
    public class TeamModel
    {
        public List<UserInvite> PendinigUsers { get; set; }
        public List<UserInvite> AcceptedUsers { get; set; }
    }
}