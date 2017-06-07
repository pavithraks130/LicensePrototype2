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
        /// <summary>
        /// Lists all the role in DB
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Create New role 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Updating Existing Role
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public IdentityResult UpdateRole(Role r)
        {
            var role = AutoMapper.Mapper.Map<DataModel.Role, Core.Model.Role>(r);
            return RoleManager.Update(role);
        }

        /// <summary>
        ///  Get Role based on ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Role GetRoleById(string id)
        {
            var r = RoleManager.FindById(id);
            return AutoMapper.Mapper.Map<Core.Model.Role, DataModel.Role>(r);
        }

        /// <summary>
        /// Delete Role By id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IdentityResult DeleteRole(string id)
        {
            var r = RoleManager.FindById(id);
            return RoleManager.Delete(r);
        }
    }
}
