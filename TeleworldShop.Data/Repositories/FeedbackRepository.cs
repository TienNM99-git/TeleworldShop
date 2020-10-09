using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleworldShop.Data.Infrastructure;
using TeleworldShop.Model.Models;

namespace TeleworldShop.Data.Repositories
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {

    }
    public class FeedbackRepository : RepositoryBase<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
