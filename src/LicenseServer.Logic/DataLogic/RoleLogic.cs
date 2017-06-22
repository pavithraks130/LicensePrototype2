using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Models;
using Microsoft.AspNet.Identity;
using LicenseServer.Core.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using LicenseServer.Core.Manager;
using LicenseServer.Core.DbContext;

namespace LicenseServer.Logic
{
    public class RoleLogic : BaseLogic
    {
        private LicRoleManager RoleManager;
        private AppDbContext _context;

        public RoleLogic()
        {
            _context = AppDbContext.Create();
            RoleManager = new LicRoleManager(new RoleStore<AppRole>(_context));
        }

        public ICollection<Role> GetRoles()
        {
            List<Role> listRoles = new List<Role>();
            var list = RoleManager.Roles.ToList();
            foreach (var r in list)
            {
                listRoles.Add(AutoMapper.Mapper.Map<LicenseServer.Core.Model.AppRole, Role>(r));
            }
            return listRoles;
        }

        public IdentityResult CreateRole(Role r)
        {
            try
            {
                var obj = AutoMapper.Mapper.Map<Role, LicenseServer.Core.Model.AppRole>(r);
                return RoleManager.Create(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IdentityResult UpdateRole(Role r)
        {
            var role = AutoMapper.Mapper.Map<Role, Core.Model.AppRole>(r);
            return RoleManager.Update(role);
        }

        public Role GetRoleById(string id)
        {
            var r = RoleManager.FindById(id);
            return AutoMapper.Mapper.Map<Core.Model.AppRole, Role>(r);
        }

        public Role GetRoleByName(string name)
        {
            var r = RoleManager.FindByName(name);
            return AutoMapper.Mapper.Map<Core.Model.AppRole, Role>(r);
        }

        public IdentityResult DeleteRole(string id)
        {
            var r = RoleManager.FindById(id);
            return RoleManager.Delete(r);
        }
    }
}
