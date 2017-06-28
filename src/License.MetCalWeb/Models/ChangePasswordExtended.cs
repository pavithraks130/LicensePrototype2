using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Models;

namespace License.MetCalWeb.Models
{
    public class ChangePasswordExtended : ChangePassword
    {

        //public ChangePassword ChangePassword { get; set; }

        //public string UserId { get { return ChangePassword.UserId; } set { ChangePassword.UserId = value; } }
        //public string Email { get { return ChangePassword.Email; } set { ChangePassword.Email = value; } }
        //public string CurrentPassword { get { return ChangePassword.CurrentPassword; } set { ChangePassword.CurrentPassword = value; } }
        //public string NewPassword { get { return ChangePassword.NewPassword; } set { ChangePassword.NewPassword = value; } }
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "New Password and Confirm Password Does not match")]
        public string ConfirmPassword { get; set; }

        //public ChangePasswordExtended()
        //{
        //    ChangePassword = new ChangePassword();
        //}
    }
}