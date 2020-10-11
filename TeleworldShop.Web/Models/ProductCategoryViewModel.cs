using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeleworldShop.Web.Models
{
    public class ProductCategoryViewModel
    {
        public int Id { set; get; }

        [Required(ErrorMessage = "Category name is required")]
        public string Name { set; get; }

        [Required(ErrorMessage = "SEO title is required")]
        public string Alias { set; get; }

        public string Description { set; get; }

        public int? ParentId { set; get; }
        public int? DisplayOrder { set; get; }

        public string Image { set; get; }

        public bool? HomeFlag { set; get; }

        public virtual IEnumerable<PostViewModel> Posts { set; get; }

        public DateTime? CreatedDate { set; get; }


        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }


        public string UpdatedBy { set; get; }


        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }

        [Required(ErrorMessage = "Status is required")]
        public bool Status { set; get; }
    }
}