using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.Manager;

namespace License.Logic.ServiceLogic
{
    public class BaseLogic
    {
        public AppUserManager UserManager { get; set; }
        public AppRoleManager RoleManager { get; set; }
    }
}
