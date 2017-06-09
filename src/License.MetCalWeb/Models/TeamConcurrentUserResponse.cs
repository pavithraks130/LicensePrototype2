﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class TeamConcurrentUserResponse
    {
        public int TeamId { get; set; }
        public bool UserUpdateStatus { get; set; }
        public string ErrorMessage { get; set; }
        public int OldUserCount { get; set; }
    }
}