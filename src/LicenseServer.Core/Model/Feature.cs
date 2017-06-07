using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.Core.Model
{
    public class Feature
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public bool IsEnabled { get; set; }
        public double Price { get; set; }
        public ICollection<Product> AssociatedProduct { get; set; }
        public SubscriptionCategory Caategory { get; set; }

    }
}
