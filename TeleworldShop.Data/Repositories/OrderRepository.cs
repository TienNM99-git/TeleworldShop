using System.Collections.Generic;
using System.Data.SqlClient;
using TeleworldShop.Common.ViewModels;
using TeleworldShop.Data.Infrastructure;
using System.Linq;
using TeleworldShop.Model.Models;

namespace TeleworldShop.Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate);

        List<PurchaseHistoryViewModel> GetOrdersByUserId(string userId);
    }

    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<RevenueStatisticViewModel>("GetRevenueStatistics @fromDate,@toDate", parameters);
        }

        public List<PurchaseHistoryViewModel> GetOrdersByUserId(string userId)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@userId",userId),
            };

            return DbContext.Database.SqlQuery<PurchaseHistoryViewModel>("GetOrdersByUserId @userId", parameters).ToList();
        }
    }
}