using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace LicenseServer.Core.Model
{
    /// <summary>
    /// class reprasents the  structure of Token class which will be mapped with user  which will be used during the registration
    /// </summary>
    public class UserToken
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }

    }
}
