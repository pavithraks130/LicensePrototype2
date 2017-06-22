using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Models
{
    public class ForgotPassword
    {
        public string Email { get; set; }
    }

    public class ForgotPasswordToken
    {
        public string UserId { get; set; }

        public string Token { get; set; }
    }
}
