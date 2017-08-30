using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Core.Model
{
    public class ClientAppVerificationSettings
    {
        public int Id { get; set; }
        public string ApplicationCode { get; set; }
        public string ApplicationSecretkey { get; set; }
    }
}
