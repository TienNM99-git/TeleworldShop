using System.Collections.Generic;
using TeleworldShop.Common.ViewModels;
using TeleworldShop.Data.Repositories;

namespace TeleworldShop.Service
{
    public interface IStatisticService
    {
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate);
    }

    public class StatisticService : IStatisticService
    {
        private IOrderRepository _orderRepository;

        public StatisticService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate)
        {
            return _orderRepository.GetRevenueStatistic(fromDate, toDate);
        }
    }
}