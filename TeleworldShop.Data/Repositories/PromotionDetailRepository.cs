using TeleworldShop.Data.Infrastructure;
using TeleworldShop.Model.Models;

namespace TeleworldShop.Data.Repositories
{
    public interface IPromotionDetailRepository : IRepository<PromotionDetail>
    {
    }
    public class PromotionDetailRepository : RepositoryBase<PromotionDetail>, IPromotionDetailRepository
    {
        public PromotionDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
