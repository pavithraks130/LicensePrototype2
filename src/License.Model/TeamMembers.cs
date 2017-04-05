﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    public class TeamMembers
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
        public bool IsAdmin { get; set; }
        public bool IsSelected { get; set; }

        public bool IsActive { get; set; }

    }

    public class UserInviteList
    {
        public User AdminUser { get; set; }
        public List<TeamMembers> PendingInvites { get; set; }
        public List<TeamMembers> AcceptedInvites { get; set; }

        public UserInviteList()
        {
            PendingInvites = new List<TeamMembers>();
            AcceptedInvites = new List<TeamMembers>();
        }
    }

    public class TeamMemberResponse
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int TeamMemberId { get; set; }
    }
}
