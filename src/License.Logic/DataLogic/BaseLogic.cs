using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.GenericRepository;
using License.Core.Manager;

namespace License.Logic.DataLogic
{
    public class BaseLogic
    {
        public UnitOfWork Work = new UnitOfWork();

        public AppUserManager UserManager { get; set; }
        public AppRoleManager RoleManager { get; set; }
     
        public String ErrorMessage { get; set; }
    }
}
