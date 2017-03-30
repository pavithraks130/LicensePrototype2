using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class RequestedLicense
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public bool IsRejected { get; set; }

        public string ApprovedBy { get; set; }
        
    }
}