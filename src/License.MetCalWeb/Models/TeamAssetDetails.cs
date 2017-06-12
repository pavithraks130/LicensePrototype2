using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class TeamAssetDetails
    {
        public int Id { get; set; }
        public string AdminId { get; set; }
        public int TeamId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SerialNumber { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
    }
}