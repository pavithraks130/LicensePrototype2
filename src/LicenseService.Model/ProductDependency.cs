using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.DataModel
{
    public class ProductDependency
    {
        public List<SubscriptionCategory> Categories { get; set; }
        public List<Feature> Features { get; set; }
    }
}
