using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.MetCalDesktop.Model
{
   public class CartItems
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public System.DateTime DateCreated { get; set; }

        public int SubscriptionTypeId { get; set; }
        public SubscriptionType SubType { get; set; }

        public string UserId { get; set; }

        public bool IsPurchased { get; set; }

        public double Price { get; set; }

        public double TotalPrice { get { return Price * Quantity; } }

    }
}
