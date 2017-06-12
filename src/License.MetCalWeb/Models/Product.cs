using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public List<SubscriptionCategory> Categories { get; set; }
        public List<Feature> Features { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ActivationMonth { get; set; }
        public string Type { get; set; }
        public bool IsSelected { get; set; }
        public bool IsDisabled { get; set; }
        public bool InitialState { get; set; }
        public DateTime ExpireDate { get; set; }
        public int AvailableCount { get; set; }
        public List<ProductAdditionalOption> AdditionalOption { get; set; }

    }

}