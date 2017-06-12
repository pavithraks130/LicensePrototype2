using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class PurchaseOrderItemDeatils
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public int Quantity { get; set; }
        public int PurchaseOrderId { get; set; }
        public Subscription Subscription { get; set; }
    }
}