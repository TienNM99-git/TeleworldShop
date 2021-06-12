using System;
using System.Collections;
using System.Collections.Generic;
using TeleworldShop.Data.Infrastructure;
using TeleworldShop.Model.Models;
using System.Linq;
using System.Linq.Expressions;

namespace TeleworldShop.Data.Repositories
{ 
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow);

    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow)
        {
            var query = from p in DbContext.Products
                        join pt in DbContext.ProductTags
                        on p.Id equals pt.ProductId
                        where pt.TagId == tagId
                        select p;
            totalRow = query.Count();
            return query.OrderByDescending(x=>x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
        }

    }
}