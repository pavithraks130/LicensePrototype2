﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace License.Core.Model
{
    public class UserLicense
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int LicenseId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }
        [ForeignKey("LicenseId")]
        public LicenseData License { get; set; }
    }
}