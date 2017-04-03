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
        public IList<string> Roles { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public string ServerUserId { get; set; }
        public bool IsActive { get; set; }
        
        public List<SubscriptionType> SubscriptionList { get; set; }
        public UserModel()
        {
            Organization = new Organization();
            SubscriptionList = new List<SubscriptionType>();
        }
    }

    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}

