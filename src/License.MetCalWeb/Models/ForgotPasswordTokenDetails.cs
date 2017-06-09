using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class ForgotPasswordTokenDetails
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}