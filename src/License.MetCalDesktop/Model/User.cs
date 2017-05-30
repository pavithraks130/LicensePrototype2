using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.MetCalDesktop.Model
{
    public class User
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public IList<string> Roles { get; set; }
        public string ServerUserId { get; set; }
        public bool IsActive { get; set; }
        public string PasswordHash { get; set; }
        public string ThumbPrint { get; set; }
    }
    public class ConcurrentUserLogin
    {
        public int TeamId { get; set; }

        public string UserId { get; set; }

        public bool IsUserLoggedIn { get; set; }

        public string ErrorOrNotificationMessage { get; set; }
    }
}
