using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Model
{
    public class TeamAsset
    { 
        public int Id { get; set; }
        public string AdminId { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
    }
}
