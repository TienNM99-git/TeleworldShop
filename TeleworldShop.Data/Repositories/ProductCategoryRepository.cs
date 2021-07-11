using System.Collections.Generic;
using System.Linq;
using TeleworldShop.Data.Infrastructure;
using TeleworldShop.Model.Models;

namespace TeleworldShop.Data.Repositories
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        IEnumerable<ProductCategory> GetByAlias(string alias);

        IEnumerable<ProductCategory> GetListCategoryByPromotionId(int promotionId);

        IEnumerable<ProductCategory> GetListAvailableCategory();
    }

    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public IEnumerable<ProductCategory> GetByAlias(string alias)
        {
            return this.DbContext.ProductCategories.Where(x => x.Alias == alias);
        }

        public IEnumerable<ProductCategory> GetListCategoryByPromotionId(int promotionId)
        {
            var query = from pc in DbContext.ProductCategories
                        join pd in DbContext.PromotionDetails
                        on pc.Id equals pd.CategoryId
                        where pd.PromotionId == promotionId
                        select pc;

            return query;
        }

        public IEnumerable<ProductCategory> GetListAvailableCategory()
        {
            var query = from pc in DbContext.ProductCategories
                        where pc.Status == true &&
                        !(from pd in DbContext.PromotionDetails
                          join promotion in DbContext.Promotions
                          on pd.PromotionId equals promotion.Id
                          where promotion.Status == true
                          select pd.CategoryId).Contains(pc.Id)
                        select pc;

            return query;
        }
    }
}