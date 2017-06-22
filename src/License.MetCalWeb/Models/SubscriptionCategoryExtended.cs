using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Models;
namespace License.MetCalWeb.Models
{
    public class SubscriptionCategoryExtended : SubscriptionCategory
    {
        public bool IsSelected { get; set; }
    }
}