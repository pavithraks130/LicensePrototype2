using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class ConcurrentUserLogin
    {
        public int TeamId { get; set; }

        public string UserId { get; set; }

        public bool IsUserLoggedIn { get; set; }

        public string ErrorOrNotificationMessage { get; set; }

        public List<Product> Products { get; set; }
    }
}