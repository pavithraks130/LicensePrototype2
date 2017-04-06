using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
	public class HardwareModel
	{
		public List<TeamAsset> Assets { get; set; }
	}

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