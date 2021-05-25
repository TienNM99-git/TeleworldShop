using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeleworldShop.Model.Models
{
    [Table("PromotionDetails")]
    public class PromotionDetail
    {
        [Key]
        [Column(Order = 1)]
        public int PromotionId { set; get; }

        [Column(Order = 2)]
        [Key]
        public int CategoryId { set; get; }

    }
}
