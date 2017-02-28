using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace LicenseServer.Core.Model
{
    public class Appuser : IdentityUser
    {
        [NotMapped]
        public string UserId
        {
            get { return base.Id; }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string Email
        {
            get { return base.Email; }
            set { base.Email = value; }
        }
        public override string PhoneNumber
        {
            get
            {
                return base.PhoneNumber;
            }
            set
            {
                base.PhoneNumber = value;
            }
        }

      
        public override string UserName
        {
            get
            {
                return base.UserName;
            }
            set
            {
                base.UserName = value;
            }
        }
         
        public int OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

        [NotMapped]
        public string Name
        {
            get
            {
                FirstName = String.IsNullOrEmpty(FirstName) ? "" : FirstName;
                LastName = String.IsNullOrEmpty(LastName) ? "" : LastName;
                return FirstName.Trim() + " " + LastName.Trim();
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Appuser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
