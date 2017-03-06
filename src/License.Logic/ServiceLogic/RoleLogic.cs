using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model;
using Microsoft.AspNet.Identity;

namespace License.Logic.ServiceLogic
{
    public class RoleLogic : BaseLogic
    {
        public ICollection<Role> GetRoles()
        {
            List<Role> listRoles = new List<Role>();
            var list = RoleManager.Roles.ToList();
            foreach (var r in list)
            {
                listRoles.Add(AutoMapper.Mapper.Map<License.Core.Model.Role, Role>(r));
            }
            return listRoles;
        }

        public IdentityResult CreateRole(Role r)
        {
            try
            {
                var obj = AutoMapper.Mapper.Map<Role, License.Core.Model.Role>(r);
                return RoleManager.Create(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IdentityResult UpdateRole(Role r)
        {
            var role = AutoMapper.Mapper.Map<Model.Role, Core.Model.Role>(r);
            return RoleManager.Update(role);
        }

        public Role GetRoleById(string id)
        {
            var r = RoleManager.FindById(id);
            return AutoMapper.Mapper.Map<Core.Model.Role, Model.Role>(r);
        }

        public IdentityResult DeleteRole(string id)
        {
            var r = RoleManager.FindById(id);
            return RoleManager.Delete(r);
        }
    }
}
