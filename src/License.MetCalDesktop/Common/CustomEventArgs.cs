using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.MetCalDesktop.Common
{
    public class CustomEventArgs : EventArgs
    {
        public bool IsConcurrentuserLoggedIn { get; set; }

        public string ErrorMessage { get; set; }
    }
}
