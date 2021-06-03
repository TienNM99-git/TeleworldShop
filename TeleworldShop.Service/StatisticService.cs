using System.Collections.Generic;
using TeleworldShop.Common.ViewModels;
using TeleworldShop.Data.Repositories;

namespace TeleworldShop.Service
{
    public interface IStatisticService
    {
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate);
        IEnumerable<OrderStatisticViewModel> GetOrderStatistic(string fromDate, string toDate);
        IEnumerable<SellStatisticViewModel> GetSellStatistic(string fromDate, string toDate);
        IEnumerable<InventoryStatisticViewModel> GetInventoryStatistic(string fromDate, string toDate);
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
        public IEnumerable<OrderStatisticViewModel> GetOrderStatistic(string fromDate, string toDate)
        {
            return _orderRepository.GetOrderStatistic(fromDate, toDate);
        }
        public IEnumerable<SellStatisticViewModel> GetSellStatistic(string fromDate, string toDate)
        {
            return _orderRepository.GetSellStatistic(fromDate, toDate);
        }

        public IEnumerable<InventoryStatisticViewModel> GetInventoryStatistic(string fromDate, string toDate)
        {
            return _orderRepository.GetInventoryStatistic(fromDate, toDate);
        }
    }
}