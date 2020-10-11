using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeleworldShop.Web.Models
{
    public class FeedbackViewModel
    {
        public int Id { set; get; }

        [MaxLength(250,ErrorMessage ="Name must not exceed 250 characters")]
        [Required(ErrorMessage ="Name is required")]
        public string Name { set; get; }

        [MaxLength(250, ErrorMessage = "Email must not exceed 250 characters")]
        public string Email { set; get; }

        [MaxLength(500, ErrorMessage = "Message must not exceed 500 characters")]
        public string Message { set; get; }

        public DateTime CreatedDate { set; get; }

        [Required(ErrorMessage ="Status is required")]
        public bool Status { set; get; }

        public ContactDetailViewModel ContactDetail { set; get; }
    }
}