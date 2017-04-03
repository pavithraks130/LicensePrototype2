using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Model;

namespace License.MetCalWeb.Models
{
	public class HardwareModel
	{
		public List<TeamAsset> Assets { get; set; }

        public TeamAsset SelectedAsset { get; set; }

        public HardwareModel()
        {
            SelectedAsset = new TeamAsset();
        }
	}
}