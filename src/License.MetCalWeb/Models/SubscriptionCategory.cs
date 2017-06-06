using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
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
}