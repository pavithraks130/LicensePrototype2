using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class Role
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsDefault { get; set; }
    }
}