using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeleworldShop.Web.Models
{
    public class ContactDetailViewModel
    {
        public int Id { set; get; }

        [Required(ErrorMessage ="Name is required")]
        public string Name { set; get; }

        [MaxLength(50, ErrorMessage = "Phone must not exceed 50 characters")]
        public string Phone { set; get; }

        [MaxLength(250, ErrorMessage = "Email must not exceed 50 characters")]
        public string Email { set; get; }

        [MaxLength(250, ErrorMessage = "Website must not exceed 50 characters")]
        public string Website { set; get; }

        [MaxLength(250, ErrorMessage = "Address must not exceed 50 characters")]
        public string Address { set; get; }

        public string Other { set; get; }

        public double? Lat { set; get; }

        public double? Lng { set; get; }

        public bool Status { set; get; }
    }
}