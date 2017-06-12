using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class ProductDependencyDetails
    {
        public List<SubscriptionCategory> Categories { get; set; }
        public List<Feature> Features { get; set; }
    }
}