using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<Feature> AssociatedFeatures { get; set; }

    }
    public class Products
    {
        public Product Product { get; set; }
        public int AvailableProductCount { get; set; }
        public bool IsSelected { get; set; }
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
}
