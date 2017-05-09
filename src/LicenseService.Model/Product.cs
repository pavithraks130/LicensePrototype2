using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.DataModel
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public string ImagePath { get; set; }
        public string CreatedDate { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public ICollection<ProductCategory> Categories { get; set; }
        public ICollection<Feature> AssociatedFeatures { get; set; }

    }

    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ProductDependency
    {
        public List<ProductCategory> Categories { get; set; }

        public List<Feature> Features { get; set; }
    }
}
