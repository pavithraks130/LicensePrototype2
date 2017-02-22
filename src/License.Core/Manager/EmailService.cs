using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace License.Core.Manager
{
    class EmailService :IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
