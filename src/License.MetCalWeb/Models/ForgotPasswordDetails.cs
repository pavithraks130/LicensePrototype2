using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class ForgotPasswordDetails
    {
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}