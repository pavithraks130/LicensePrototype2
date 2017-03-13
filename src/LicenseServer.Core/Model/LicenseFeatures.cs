using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.Core.Model
{
    public class LicenseFeatures
    {

        [Key]
        public int FeatureId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> AssociatedProduct { get; set; }

    }
}
