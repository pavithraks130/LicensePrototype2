using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    public class UserSubscription
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int SubscriptionId { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public User User { get; set; }

        public SubscriptionType Subscription { get; set; }

        public List<LicenseData> LicenseList { get; set; }

        public int Quantity { get; set; }
    }
   
    public class SubscriptionDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserSubscriptionId { get; set; }
        public List<ProductDetails> Products { get; set; }
        public SubscriptionDetails()
        {
            Products = new List<ProductDetails>();
        }
    }

    public class ProductDetails
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public String Description { get; set; }
        public int TotalLicenseCount { get; set; }
        public int UsedLicenseCount { get; set; }
        public bool IsSelected { get; set; }
        public bool IsDisabled { get; set; }
        public bool InitialState { get; set; }
        public List<Feature> Features { get; set; }
        public DateTime ExpireDate { get; set; }
        public ProductDetails()
        {
            Features = new List<Feature>();
        }
    }
    
    #region SyncSubscription
    public class UserSubscriptionData
    {
        public string UserId { get; set; }

        public int SubscriptionId { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public Subscription Subscription { get; set; }

        public int Quantity { get; set; }

        public List<LicenseKeyProductMapping> LicenseKeys { get; set; }
    }

    public class Subscription
    {
        public SubscriptionType SubscriptionType { get; set; }

        public List<LicenseKeyProductMapping> LicenseKeyProductMapping { get; set; }

        public Subscription()
        {
            LicenseKeyProductMapping = new List<LicenseKeyProductMapping>();
        }

        public int OrderdQuantity { get; set; }

    }

    public class SubscriptionType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public List<Feature> AssociatedFeatures { get; set; }

    }

    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
    }

    public class LicenseKeyProductMapping
    {
        public string LicenseKey { get; set; }
        public int ProductId { get; set; }

    }
    #endregion

}
