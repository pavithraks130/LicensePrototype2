using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public bool IsEnabled { get; set; }
        public double Price { get; set; }
    }
}
