using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Models;

namespace License.MetCalDesktop.Model
{
    public class SubscriptionExtended : Subscription
    {
        //public Subscription Subscription { get; set; }
        //public int Id { get { return Subscription.Id; }  set { Subscription.Id = value; } }
        //public string Name { get { return Subscription.Name; } set { Subscription.Name = value; } }
        //public string ImagePath { get { return Subscription.ImagePath; } set { Subscription.ImagePath = value; } }
        //public int ActiveDays { get { return Subscription.ActiveDays; } set { Subscription.ActiveDays = value; } }
        //public double Price { get { return Subscription.Price; } set { Subscription.Price = value; } }
        //public string CreatedBy { get { return Subscription.CreatedBy; } set { Subscription.CreatedBy = value; } }
        //public int UserSubscriptionId { get { return Subscription.UserSubscriptionId; } set { Subscription.UserSubscriptionId = value; } }
        //public SubscriptionCategory Category { get { return Subscription.Category; } set { Subscription.Category = value; } }
        //public ICollection<Product> Products { get { return Subscription.Products; } set { Subscription.Products = value; } }
        //public List<dynamic> ProductIdList { get { return Subscription.ProductIdList; } set { Subscription.ProductIdList = value; } }
        public int ActivationMonth { get; set; }
        public int NoOfUsers { get; set; }
        public SubscriptionExtended() { }
        public SubscriptionExtended(Subscription s)
        {
            Id = s.Id;
            Name = s.Name;
            ImagePath = s.ImagePath;
            ActiveDays = s.ActiveDays;
            Price = s.Price;
            CreatedBy = s.CreatedBy;
            UserSubscriptionId = s.UserSubscriptionId;
            Category = s.Category;
            Products = s.Products;
            ProductIdList = s.ProductIdList;
        }
    }
}
