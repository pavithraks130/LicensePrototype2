using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Models;
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
                listRoles.Add(AutoMapper.Mapper.Map<Role>(r));
            }
            return listRoles.OrderBy(r => r.Name).ToList();
        }

        /// <summary>
        /// Create New role 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public Role CreateRole(Role r)
        {
            try
            {
                var role = RoleManager.FindByName(r.Name);
                if (role == null)
                {
                    var obj = AutoMapper.Mapper.Map<Role, License.Core.Model.Role>(r);
                    var result = RoleManager.Create(obj);
                    if (result.Succeeded)
                    {
                        var roleObj = RoleManager.FindByName(r.Name);
                        return AutoMapper.Mapper.Map<Role>(roleObj);
                    }
                    else
                        ErrorMessage = result.Errors.ToList().ToString();
                }
                else
                    ErrorMessage = "Role already Exist";

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return null;
        }

        /// <summary>
        /// Updating Existing Role
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public Role UpdateRole(string id, Role r)
        {
            var role = RoleManager.FindById(id);
            role.Name = r.Name;
            var duplicateRole = RoleManager.FindByName(r.Name);
            if (duplicateRole != null && role.Id != duplicateRole.Id)
                ErrorMessage = "Role Exist in this name";
            else
            {
                var result = RoleManager.Update(role);
                if (result.Succeeded)
                {
                    var roleObj = RoleManager.FindById(id);
                    return AutoMapper.Mapper.Map<Role>(roleObj);
                }
                else
                    ErrorMessage = result.Errors.ToList().ToString();
            }
            return null;
        }

        /// <summary>
        ///  Get Role based on ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Role GetRoleById(string id)
        {
            var r = RoleManager.FindById(id);
            return AutoMapper.Mapper.Map<Role>(r);
        }

        /// <summary>
        /// Delete Role By id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Role DeleteRole(string id)
        {
            var r = RoleManager.FindById(id);
            var result =  RoleManager.Delete(r);
            if (result.Succeeded)
                return AutoMapper.Mapper.Map<Role>(r);
            else
                ErrorMessage = result.Errors.ToList().ToString();
            return null;
        }
    }
}
