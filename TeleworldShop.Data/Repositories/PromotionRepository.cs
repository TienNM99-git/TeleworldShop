using TeleworldShop.Data.Infrastructure;
using TeleworldShop.Model.Models;

namespace TeleworldShop.Data.Repositories
{
    public interface IPromotionRepository : IRepository<Promotion>
    {
    }

    public class PromotionRepository : RepositoryBase<Promotion>, IPromotionRepository
    {
        public PromotionRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}