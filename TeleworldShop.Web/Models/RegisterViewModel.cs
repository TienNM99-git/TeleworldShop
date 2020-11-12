using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeleworldShop.Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Your fullname is required")]
        public string FullName { set; get; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must have atleast 6 letters")]
        public string Password { set; get; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email invalid")]
        public string Email { set; get; }

        public string Address { set; get; }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { set; get; }

    }
}