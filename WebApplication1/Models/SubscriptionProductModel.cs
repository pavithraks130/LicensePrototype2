using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class SubscriptionProductModel
    {
        public String SubscriptionName { get; set; }

        public List<ProductDetails> ProductDtls { get; set; }

        public SubscriptionProductModel()
        {
            ProductDtls = new List<ProductDetails>();
        }
    }

    public class ProductDetails
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int AvailableCount
        {
            get { return TotalCount - UsedLicenseCount; }
        }
        public int TotalCount { get; set; }
        public int UsedLicenseCount { get; set; }
    }
}