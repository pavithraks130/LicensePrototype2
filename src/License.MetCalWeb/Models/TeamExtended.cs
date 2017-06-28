using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using License.Models;
namespace License.MetCalWeb.Models
{
    public class TeamExtended:Team
    {
        //public Team Team { get; set; }
        //public int Id { get { return Team.Id; } set { Team.Id = value; } }
        //public string Name { get { return Team.Name; } set { Team.Name = value; } }
        //public string AdminId { get { return Team.AdminId; } set { Team.AdminId = value; } }
        //public User AdminUser { get { return Team.AdminUser; } set { Team.AdminUser = value; } }
        //public bool IsDefaultTeam { get { return Team.IsDefaultTeam; } set { Team.IsDefaultTeam = value; } }
        //public int ConcurrentUserCount { get { return Team.ConcurrentUserCount; } set { Team.ConcurrentUserCount = value; } }
        //public ICollection<TeamMember> TeamMembers { get { return Team.TeamMembers; } set { Team.TeamMembers = value; } }
        public bool IsSelected { get; set; }
        public TeamExtended() { }
        public TeamExtended(Team t )
        {
            Id = t.Id;
            Name = t.Name;
            AdminId = t.Name;
            AdminUser = t.AdminUser;
            IsDefaultTeam = t.IsDefaultTeam;
            ConcurrentUserCount = t.ConcurrentUserCount;
            TeamMembers = t.TeamMembers;
        }
    }

}