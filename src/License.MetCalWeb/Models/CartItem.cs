using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public System.DateTime DateCreated { get; set; }

        public int SubscriptionId { get; set; }
        public Subscription SubType { get; set; }

        public string UserId { get; set; }

        public bool IsPurchased { get; set; }

        public double Price { get; set; }

        public double TotalPrice { get { return Price * Quantity; } }
    }
}