using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.Models;

namespace License.MetCalWeb.Models
{
    public class RegistrationExtended :Registration
    {
        //public Registration Registration { get; set; }
        //public string FirstName { get { return Registration.FirstName; } set { Registration.FirstName = value; } }
        //public string LastName { get { return Registration.LastName; } set { Registration.LastName = value; } }
        //public string Email { get { return Registration.Email; } set { Registration.Email = value; } }
        //public string PhoneNumber { get { return Registration.PhoneNumber; } set { Registration.PhoneNumber = value; } }
        //public string OrganizationName { get { return Registration.OrganizationName; } set { Registration.OrganizationName = value; } }
        //public string Password { get { return Registration.Password; } set { Registration.Password = value; } }
        //public string Token { get { return Registration.Token; } set { Registration.Token = value; } }
        //public string ServerUserId { get { return Registration.ServerUserId; } set { Registration.ServerUserId = value; } }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //public RegistrationExtended()
        //{
        //    Registration = new Registration();
        //}
        
    }

}