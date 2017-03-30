using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    /// <summary>
    /// This entire region is used to deserialize the Json data which contains the Subscription
    /// with Product details to Object which can be used in the application.
    /// </summary>
    #region XMLfileDataStruucture 
    public class SubscriptionProductModel
    {
        public int SubscriptionId { get; set; }
        public String SubscriptionName { get; set; }

        public List<ProductDetails> ProductDtls { get; set; }

        public SubscriptionProductModel()
        {
            ProductDtls = new List<ProductDetails>();
        }
    }

    public class ProductDetails
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int AvailableCount
        {
            get { return TotalCount - UsedLicenseCount; }
        }
        public int TotalCount { get; set; }
        public int UsedLicenseCount { get; set; }
    }
    
    #endregion
}
