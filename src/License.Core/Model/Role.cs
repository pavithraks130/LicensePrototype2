using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace License.Core.Model
{
    public class Role : IdentityRole
    {
        public string Name { get { return base.Name; } set { base.Name = value; } }

        public bool IsDefault { get; set; }
    }


}
