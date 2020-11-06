using System;
using System.Runtime.Serialization;

namespace TeleworldShop.Web.Models
{
    [Serializable]
    [DataContract]
    public class ProductViewModel
    {
        [DataMember]
        public int Id { set; get; }

        [DataMember]
        public string Name { set; get; }

        [DataMember]
        public string Alias { set; get; }

        [DataMember]
        public int CategoryId { set; get; }

        [DataMember]
        public string Image { set; get; }

        [DataMember]
        public string MoreImages { set; get; }

        [DataMember]
        public decimal Price { set; get; }

        [DataMember]
        public decimal? PromotionPrice { set; get; }

        [DataMember]
        public int? Warranty { set; get; }

        [DataMember]
        public string Description { set; get; }

        [DataMember]
        public string Content { set; get; }

        [DataMember]
        public bool? HomeFlag { set; get; }

        [DataMember]
        public bool? HotFlag { set; get; }

        [DataMember]
        public int? ViewCount { set; get; }

        [DataMember]
        public DateTime? CreatedDate { set; get; }

        [DataMember]
        public string CreatedBy { set; get; }

        [DataMember]
        public DateTime? UpdatedDate { set; get; }

        [DataMember]
        public string UpdatedBy { set; get; }

        [DataMember]
        public string MetaKeyword { set; get; }

        [DataMember]
        public string MetaDescription { set; get; }

        [DataMember]
        public bool Status { set; get; }

        [DataMember]
        public string Tags { set; get; }

        [DataMember]
        public int Quantity { set; get; }

        [DataMember]
        public decimal OriginalPrice { set; get; }

        public virtual ProductCategoryViewModel ProductCategory { set; get; }
    }
}