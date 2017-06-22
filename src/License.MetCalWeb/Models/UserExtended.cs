using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using License.Models;

namespace License.MetCalWeb.Models
{
    public class UserExtended : User
    {
        public List<Role> RolesList { get;set;}
    }
}

