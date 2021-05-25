using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeleworldShop.Model.Abstract;

namespace TeleworldShop.Model.Models
{
    [Table("Promotions")]
    public class Promotion : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [Required]
        public int Type { set; get; }

        [Required]
        public decimal? PromotionPrice { set; get; }

        [Required]
        public int Apply { set; get; }

        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? ExpireDate { get; set; }

        public IEnumerable<PromotionDetail> PromotionDetails { set; get; }
    }
}
