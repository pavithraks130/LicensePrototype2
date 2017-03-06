using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.DataModel
{
    /// <summary>
    /// class is used for synching the subscription data from LicenseServer to OnPremise Server
    /// </summary>
    public class ProductSubscriptionDtls
    {
        public int SubscriptionTypeId { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public SubscriptionType SubscriptionType { get; set; }

        public List<ProductDetails> Products { get; set; }

        public List<LicenseKeyProductMapping> LicenseKeyProductMapping { get; set; }

        public ProductSubscriptionDtls()
        {
            Products = new List<ProductDetails>();
            LicenseKeyProductMapping = new List<DataModel.LicenseKeyProductMapping>();
        }

        public int OrderdQuantity { get; set; }
    }

    public class ProductDetails
    {
        public Product Product { get; set; }
        public int QtyPerSubscription { get; set; }
    }

    public class UserSubscriptionList
    {
        public string UserId { get; set; }

        public List<ProductSubscriptionDtls> SubscriptionList { get; set; }

        public UserSubscriptionList()
        {
            SubscriptionList = new List<ProductSubscriptionDtls>();
        }
    }
}
