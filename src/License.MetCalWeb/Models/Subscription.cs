using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
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
        public List<ProductDetails> Products { get; set; }
    }
}