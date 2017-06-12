using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class ResetPassword
    {
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "New Password and Confirm Password not matching")]
        public string ConfirmPassword { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }
    }
}