using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace License.MetCalWeb.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Name { get { return FirstName + " " + LastName; } }
        public IList<String> Roles { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public string ServerUserId { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
        public List<Subscription> SubscriptionList { get; set; }
        public List<Role> RolesList { get;set;}
        public User()
        {
            Organization = new Organization();
            SubscriptionList = new List<Subscription>();
        }
    }
}

