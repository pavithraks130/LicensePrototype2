using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using License.Model.Model;

namespace License.MetCalWeb.Models
{
    public class RegisterViewModel
    {
        public Registration RegistratoinModel = new Registration();

        [Required]
        [Display(Name = "FirstName")]
        public string FName { get { return RegistratoinModel.FirstName; } set { RegistratoinModel.FirstName = value; } }

        [Required]
        [Display(Name = "LastName")]
        public string LName { get { return RegistratoinModel.LastName; } set { RegistratoinModel.LastName = value; } }


        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get { return RegistratoinModel.Email; } set { RegistratoinModel.Email = value; } }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password
        {
            get { return RegistratoinModel.Password; }
            set
            {
                RegistratoinModel.Password = value;
            }
        }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        [Display(Name = "Contact Number")]
        public string PhoneNumber { get { return RegistratoinModel.PhoneNumber; } set { RegistratoinModel.PhoneNumber = value; } }

        [Required]
        [Display(Name = "Organization")]
        public string Organization
        {
            get { return RegistratoinModel.OrganizationName; }
            set
            {
                RegistratoinModel.OrganizationName = value;
            }
        }


    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }




}