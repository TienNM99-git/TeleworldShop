using System;
using System.Collections;
using System.Collections.Generic;
using TeleworldShop.Data.Infrastructure;
using TeleworldShop.Model.Models;
using System.Linq;

namespace TeleworldShop.Data.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        IEnumerable<Post> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow);
    }

    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Post> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow)
        {
            //Join 2 table to get posts that has the input tag
            var query = from p in DbContext.Posts
                        join pt in DbContext.PostTags
                        on p.Id equals pt.PostId
                        where pt.TagId == tag && p.Status
                        orderby p.CreatedDate descending
                        select p;

            //Number of rows equals to number of posts selected
            totalRow = query.Count();

            //Page 1, pageSize = 5 => get the first 5 posts in query
            //Page 2, pageSize = 5 => skip first 5 post in query, take the next 5 posts 
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return query;
        }
    }
}