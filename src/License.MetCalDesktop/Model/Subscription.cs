using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalDesktop.Model
{
    /// <summary>
    /// This entire region is used to sync the subscription details between 2 servers.
    /// </summary>
    ///   
    public class SubscriptionList
    {
        public string UserId { get; set; }

        public List<SubscriptionLicenseMapping> Subscriptions { get; set; }

        public SubscriptionList()
        {
            Subscriptions = new List<SubscriptionLicenseMapping>();
        }
    }
    public class SubscriptionLicenseMapping
    {
        public int SubscriptionId { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public DateTime RenewalDate { get; set; }
        public Subscription Subscription { get; set; }
        public DateTime ExpireDate { get; set; }
        public int UserSubscriptionId { get; set; }
        public List<LicenseKeyProductMapping> LicenseKeyProductMapping { get; set; }

        public SubscriptionLicenseMapping()
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

    public class UserSubscription
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int SubscriptionId { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public DateTime RenewalDate { get; set; }

        public Subscription Subtype { get; set; }
        public User User { get; set; }

        public int Quantity { get; set; }

    }


    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
    }
    public class FetchUserSubscription
    {
        public int TeamId { get; set; }
        public string UserId { get; set; }
        public bool IsFeatureRequired { get; set; }
    }
    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int ActiveDays { get; set; }
        public int ActivationMonth { get; set; }
        public double Price { get; set; }
        public SubscriptionCategory Category { get; set; }
        public int NoOfUsers { get; set; }
        public string CreatedBy { get; set; }
        public List<Product> Products { get; set; }
    }
    public class SubscriptionCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
        public ICollection<Product> Products { get; set; }
        public int ActivationMonth { get; set; }
        public double Price { get; set; }
    }

    public class ProductAdditionalOption
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public int ProductId { get; set; }
    }
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