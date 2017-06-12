using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace License.MetCalWeb.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string AdminId { get; set; }
        public User AdminUser { get; set; }
        public bool IsDefaultTeam { get; set; }
        public bool IsSelected { get; set; }
        public int ConcurrentUserCount { get; set; }
        public ICollection<TeamMember> TeamMembers { get; set; }
    }

}