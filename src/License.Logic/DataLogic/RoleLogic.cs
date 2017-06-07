using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.DataModel;
using Microsoft.AspNet.Identity;

namespace License.Logic.DataLogic
{
    /// <summary>
    /// History 
    /// Created By : 
    /// Created Date :
    /// Purpose : Performing CRUD functionality on the Role Table
    /// </summary>
    public class RoleLogic : BaseLogic
    {
        // Lists all the role in DB
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

        // Create New role 
        public IdentityResult CreateRole(Role r)
        {
            try
            {
                var obj = AutoMapper.Mapper.Map<Role, License.Core.Model.Role>(r);
                return RoleManager.Create(obj);
            }
            catch (Exception ex)
            {
                // throw ex;
                var result = new IdentityResult(new string[] { ex.Message });
                return result;
            }
        }

        //Updating Existing Role
        public IdentityResult UpdateRole(Role r)
        {
            var role = AutoMapper.Mapper.Map<DataModel.Role, Core.Model.Role>(r);
            return RoleManager.Update(role);
        }

        // Get Role based on ID

        public Role GetRoleById(string id)
        {
            var r = RoleManager.FindById(id);
            return AutoMapper.Mapper.Map<Core.Model.Role, DataModel.Role>(r);
        }

        // Delete Role By id
        public IdentityResult DeleteRole(string id)
        {
            var r = RoleManager.FindById(id);
            return RoleManager.Delete(r);
        }
    }
}
