using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Models;

namespace License.MetCalDesktop.Model
{
    public class UserExtended:User
    {
        public string PasswordHash { get; set; }
        public string ThumbPrint { get; set; }
    }
}
