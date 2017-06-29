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
        /// <summary>
        /// Used in the Response to get the User Id.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Used in the Response to get the Token based on the UserId4.
        /// </summary>
        public string Token { get; set; }
    }

}
