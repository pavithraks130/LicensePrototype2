using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.DataModel
{
    public class Features
    {

        public int FeatureId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> AssociatedProduct { get; set; }

    }
}
