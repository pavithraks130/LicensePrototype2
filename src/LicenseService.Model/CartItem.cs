using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.DataModel
{
    public class CartItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public System.DateTime DateCreated { get; set; }

        public int SubscriptionTypeId { get; set; }
        public SubscriptionType SubType { get; set; }

        public string UserId { get; set; }

    }
}
