using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalDesktop.Model
{
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
}