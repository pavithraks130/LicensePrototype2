using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace License.MetCalWeb.Models
{
    public class Feature
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public bool IsEnabled { get; set; }
        public bool Selected { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }

    }
}