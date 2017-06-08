using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    public class ForgotPasswordToken
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
