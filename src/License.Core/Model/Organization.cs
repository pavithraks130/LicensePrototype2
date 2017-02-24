using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Core.Model
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GroupEmail { get; set; }
        public string ContactNumber { get; set; }
    }
}
