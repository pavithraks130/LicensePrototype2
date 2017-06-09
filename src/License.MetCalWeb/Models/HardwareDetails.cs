using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace License.MetCalWeb.Models
{
    public class HardwareDetails
    {
        public List<TeamAssetDetails> Assets { get; set; }

        public HardwareDetails()
        {
            Assets = new List<TeamAssetDetails>();
        }
    }
}
