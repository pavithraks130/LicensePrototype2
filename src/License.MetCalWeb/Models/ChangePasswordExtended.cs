using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Models;

namespace License.MetCalWeb.Models
{
    public class ChangePasswordExtended : ChangePassword
    {

        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "New Password and Confirm Password Does not match")]
        public string ConfirmPassword { get; set; }
    }
}