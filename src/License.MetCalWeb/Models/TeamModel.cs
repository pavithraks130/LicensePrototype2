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

    public class DeleteTeamDetails
    {
        public string LogInUserId { get; set; }
        public int TeamId { get; set; }
        public List<int> productIdList { get; set; }
        public DeleteTeamDetails()
        {
            productIdList = new List<int>();
        }
    }

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
    }
    
    public class TeamMemberResponse
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        public int TeamMemberId { get; set; }
    }

    public class TeamConcurrentUserResponse
    {
        public int TeamId { get; set; }
        public bool UserUpdateStatus { get; set; }
        public string ErrorMessage { get; set; }
        public int OldUserCount { get; set; }
    }
}