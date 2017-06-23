using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Models;
namespace License.MetCalWeb.Models
{
    public class SubscriptionCategoryExtended  :SubscriptionCategory
    {
        //public SubscriptionCategory SubscriptionCategory { get; set; }
        //public int Id { get { return SubscriptionCategory.Id; } set { SubscriptionCategory.Id = value; } }
        //public string Name { get { return SubscriptionCategory.Name; } set { SubscriptionCategory.Name = value; } }
        //public string Description { get { return SubscriptionCategory.Description; } set { SubscriptionCategory.Description = value; } }

        public bool IsSelected { get; set; }

        public SubscriptionCategoryExtended() { }
        public SubscriptionCategoryExtended(SubscriptionCategory c)
        {
            Id = c.Id;
            Name = c.Name;
            Description = c.Description;
        }
    }
}