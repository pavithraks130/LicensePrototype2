using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.Core.Model
{
    /// <summary>
    /// This is used to create notification table on DB.
    /// </summary>
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string NotificationData { get; set; }
        public string Image { get; set; }

    }
}
