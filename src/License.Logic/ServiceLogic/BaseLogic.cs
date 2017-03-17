using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.GenericRepository;
using License.Core.Manager;

namespace License.Logic.ServiceLogic
{
    public class BaseLogic
    {
        public UnitOfWork Work = new UnitOfWork();

        private Core.DBContext.ApplicationDbContext _context = Core.DBContext.ApplicationDbContext.Create();

        private AppUserManager _userManager = null;
        protected AppUserManager UserManager
        {
            get { return _userManager ?? (_userManager = AppUserManager.Create(_context)); }
        }

        private AppRoleManager _roleManager = null;
        protected AppRoleManager RoleManager
        {
            get { return _roleManager ?? (_roleManager = AppRoleManager.Create(_context)); }
        }

        public String ErrorMessage { get; set; }
    }
}
