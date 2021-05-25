using System;
using System.Collections.Generic;

namespace TeleworldShop.Web.Models
{
    public class PromotionViewModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public int Type { set; get; }

        public decimal? PromotionPrice { set; get; }

        public int Apply { set; get; }

        public DateTime StartDate { get; set; }

        public DateTime ExpireDate { get; set; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }

        public bool Status { set; get; }

        public IEnumerable<ProductCategoryViewModel> Categories { set; get; }

        public IEnumerable<TagViewModel> Tags { set; get; }
    }
}