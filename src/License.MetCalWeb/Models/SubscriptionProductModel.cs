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
    /// 

    public class UserSubscriptionList
    {
        public string UserId { get; set; }

        public List<Subscription> SubscriptionList { get; set; }

        public UserSubscriptionList()
        {
            SubscriptionList = new List<Subscription>();
        }
    }

    public class Subscription
    {
        public int SubscriptionTypeId { get; set; }

        public DateTime SubscriptionDate { get; set; }
        public SubscriptionType SubscriptionType { get; set; }

        public List<LicenseKeyProductMapping> LicenseKeyProductMapping { get; set; }

        public Subscription()
        {
            LicenseKeyProductMapping = new List<LicenseKeyProductMapping>();
        }

        public int OrderdQuantity { get; set; }
    }

    public class LicenseKeyProductMapping
    {
        public string LicenseKey { get; set; }
        public int ProductId { get; set; }

    }

    public class SubscriptionType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<SubscriptionDetails> SubDetails { get; set; }
    }

    public class SubscriptionDetails
    {
        public int Id { get; set; }

        public int SubscriptionTypeId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public Product Product { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public ICollection<Feature> AssociatedFeatures { get; set; }

    }

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
