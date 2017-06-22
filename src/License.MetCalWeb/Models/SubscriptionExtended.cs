using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Models;

namespace License.MetCalWeb.Models
{
    public class SubscriptionExtended: Subscription
    {

        public int ActivationMonth { get; set; }
        public int NoOfUsers { get; set; }
    }
}