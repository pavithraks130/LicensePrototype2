using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.DataModel
{
    public class User
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public int OrganizationId { get; set; }

        public Organization Organization { get; set; }

        public IList<string> Roles { get; set; }
        public IList<SubscriptionType> SubscriptionList { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public User()
        {
            Roles = new List<string>();
            SubscriptionList = new List<SubscriptionType>();
        }

    }

    public class ChangePassword
    {
        public string UserId { get; set; }
        public string Email { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
