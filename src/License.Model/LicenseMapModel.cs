using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    public class LicenseMapModel
    {
        public string SubscriptionName { get; set; }

        public int UserSubscriptionId { get; set; }

        public bool InitialSelected { get; set; }

        public List<SubscriptionProduct> ProductList { get; set; }

        public bool IsSelected { get; set; }

        public bool IsDisabled { get; set; }

        public LicenseMapModel()
        {
            ProductList = new List<SubscriptionProduct>();
        }
    }

    public class SubscriptionProduct
    {
        public string ProductName { get; set; }

        public int ProductId { get; set; }

        public bool IsSelected { get; set; }

        public bool IsDisabled { get; set; }

        public bool InitialState { get; set; }

        public List<Feature> Features { get; set; }

        public DateTime ExpireDate { get; set; }

        public SubscriptionProduct()
        {
            Features = new List<Feature>();
        }
    }


}
