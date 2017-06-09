using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class UserToken
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        public string Token { get; set; }
    }

}