using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.ServiceInvoke
{
    public class Login
    {
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password), Required]
        public string Password { get; set; }


        public bool RememberMe { get; set; }
    }
}
