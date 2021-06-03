﻿using System.Collections.Generic;
using System.Data.SqlClient;
using TeleworldShop.Common.ViewModels;
using TeleworldShop.Data.Infrastructure;
using TeleworldShop.Model.Models;

namespace TeleworldShop.Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate);
        IEnumerable<OrderStatisticViewModel> GetOrderStatistic(string fromDate, string toDate);
        IEnumerable<SellStatisticViewModel> GetSellStatistic(string fromDate, string toDate);
        IEnumerable<InventoryStatisticViewModel> GetInventoryStatistic(string fromDate, string toDate);
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
        public IEnumerable<OrderStatisticViewModel> GetOrderStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<OrderStatisticViewModel>("GetOrderStatistics @fromDate, @toDate", parameters);
        }
        public IEnumerable<SellStatisticViewModel> GetSellStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<SellStatisticViewModel>("GetSellStatistics @fromDate, @toDate", parameters);
        }
        public IEnumerable<InventoryStatisticViewModel> GetInventoryStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<InventoryStatisticViewModel>("GetInventoryStatistics @fromDate, @toDate", parameters);
        }
    }
}