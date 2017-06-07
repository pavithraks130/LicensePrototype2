using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LicenseServer.DataModel
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int ActiveDays { get; set; }
        public double Price { get; set; }
        public string CreatedBy { get; set; }        
        public SubscriptionCategory Category { get; set; }
        public IEnumerable<Product> Products { get; set; }
        
    }
   
    public class RenewSubscriptionList
    {
        public DateTime RenewalDate { get; set; }
        public double Price { get; set; }
        public int RenewDuration { get; set; }
        public List<Subscription> SubscriptionList { get; set; }
    }
}
