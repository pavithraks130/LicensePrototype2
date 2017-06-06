using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace License.MetCalWeb.Models
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

    public class RenewSubscriptionList
    {
        public DateTime RenewalDate { get; set; } = new DateTime(1900, 01, 01);

        public double Price { get; set; }

        public int RenewDuration { get; set; }

        public List<Subscription> SubscriptionList { get; set; }
    }

    //public class SubscriptionDetails
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public int UserSubscriptionId { get; set; }
    //    public List<ProductDetails> Products { get; set; }
    //    public SubscriptionDetails()
    //    {
    //        Products = new List<ProductDetails>();
    //    }
    //}

    //public class Products
    //{
    //    public Product Product{ get; set; }
    //    public int AvailableProductCount { get; set; }
    //    public bool IsSelected { get; set; }
    //}

    //public class TeamMappingDetails
    //{
    //    public int ConcurrentUserCount { get; set; }
    //    public string SelectedTeamName { get; set; }
    //    public List<Products> ProductList { get; set; }
    //    public TeamMappingDetails()
    //    {
    //        ProductList = new List<Products>();
    //    }
    //}

    //public class TeamDetails
    //{
    //    public string SelectedTeamName { get; set; }
    //    public List<Product> ProductList { get; set; }
    //    public TeamDetails()
    //    {
    //        ProductList = new List<Product>();
    //    }
    //}






}
