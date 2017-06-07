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

    public class TeamMappingDetails
    {
        public int TeamID { get; set; }
        public int ConcurrentUserCount { get; set; }
        public string SelectedTeamName { get; set; }
        
        public List<Product> ProductList { get; set; }
    }
    public class TeamDetails
    {
        public Team Team { get; set; }
        public List<TeamMember> PendinigUsers { get; set; }
        public List<TeamMember> AcceptedUsers { get; set; }

        public List<Product> ProductList { get; set; }

        public List<UserLicenseRequest> LicenseRequestList { get; set; }
        public TeamDetails()
        {
            ProductList = new List<Product>();
        }
    }

    public class TeamLicenseDataMapping
    {
        public List<Team> TeamList { get; set; }

        public List<ProductLicense> LicenseDataList { get; set; }
    }


    public class TeamConcurrentUserResponse
    {
        public int TeamId { get; set; }
        public bool UserUpdateStatus { get; set; }
        public string ErrorMessage { get; set; }
        public int OldUserCount { get; set; }
    }
}