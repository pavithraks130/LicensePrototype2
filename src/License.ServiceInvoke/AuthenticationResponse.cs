using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.ServiceInvoke
{
    public class AuthenticationResponse<T>
    {

        public AccessToken OnPremiseToken { get; set; }

        public T User { get; set; }

        public AccessToken CentralizedToken { get; set; }

        public string ErrorMessage { get; set; }
    }
}
