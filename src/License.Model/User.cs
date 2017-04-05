using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    public class User
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public IList<string> Roles { get; set; }
        public string ServerUserId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ChangePassword
    {
        public string UserId { get; set; }
        public string Email { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }

    public class ResetPassword
    {
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }
    }

    public class ForgotPasswordToken
    {
        public string UserId { get; set; }

        public string Token { get; set; }
    }
}
