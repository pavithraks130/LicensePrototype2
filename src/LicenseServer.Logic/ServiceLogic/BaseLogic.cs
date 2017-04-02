using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.Core.Infrastructure;
using LicenseServer.Core.Manager;

namespace LicenseServer.Logic
{
    public class BaseLogic
    {
        public UnitOfWork Work { get; set; }

        public LicUserManager UserManager { get; set; }
        public LicRoleManager RoleManager { get; set; }

        public string ErrorMessage { get; set; }
        public BaseLogic()
        {
            Work = new UnitOfWork();
        }
    }
}
