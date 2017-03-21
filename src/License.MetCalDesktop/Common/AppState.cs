using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model;

namespace License.MetCalDesktop.Common
{
    public class AppState
    {
        private static AppState _instance = null;
        public static AppState Instance
        {
            get { return _instance ?? (_instance = new AppState()); }
        }

        public List<LicenseMapModel> UserLicenseList { get; set; }

        public bool IsUserLoggedIn { get; set; }

        public User User { get; set; }
    }
}
