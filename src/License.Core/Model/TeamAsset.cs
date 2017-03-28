using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace License.Core.Model
{
	public class TeamAsset
	{
		[Key]
		public int Id { get; set; }
		public string AdminId { get; set; }
		public int TeamId { get; set; }
		public string Name { get; set; }
		public string SerialNumber { get; set; }
		public string Type { get; set; }
		public string Description { get; set; }
	}
}
