using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ManagerId { get; set; }
        public IList<string> Roles { get; set; }
        public string OrganizationName { get; set; }
        public List<Subscription> SubscriptionList { get; set; }
        public string ServerUserId { get; set; }

        public UserModel()
        {
            SubscriptionList = new List<Subscription>();
        }
    }

    public class Subscription
    {
        public string Name { get; set; }    
        public string Description { get; set; }
    }
}