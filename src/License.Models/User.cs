using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Models
{
    public class User
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get { return FirstName + " " + LastName; } }

        public int OrganizationId { get; set; }

        public Organization Organization { get; set; }

        public IList<string> Roles { get; set; }
        public IList<Subscription> SubscriptionList { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string ServerUserId { get; set; }

        public string ThumbPrint { get; set; }

        public string PasswordHash { get; set; }
        public User()
        {
            Roles = new List<string>();
            SubscriptionList = new List<Subscription>();
        }

    }
}
