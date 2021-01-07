using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleworldShop.Common.ViewModels;
using TeleworldShop.Data.Infrastructure;
using TeleworldShop.Data.Repositories;
using TeleworldShop.Model.Models;

namespace TeleworldShop.Service
{
    public interface IOrderService
    {
        Order Create(ref Order order, List<OrderDetail> orderDetails);
        void UpdateStatus(int orderId);
        void Save();
        IEnumerable<Order> GetAll();
        void Update(Order product);
        Order Delete(int id);
        IEnumerable<Order> GetAll(string keyword);
        Order GetById(int id);
        IEnumerable<Order> Search(string keyword, int page, int pageSize, string sort, out int totalRow);
    }
    public class OrderService : IOrderService
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;
        IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
        }
        public Order Create(ref Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                _orderRepository.Add(order);
                _unitOfWork.Commit();

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderId = order.Id;
                    _orderDetailRepository.Add(orderDetail);
                }
                return order;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateStatus(int orderId)
        {
            var order = _orderRepository.GetSingleById(orderId);
            order.Status = true;
            _orderRepository.Update(order);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Order> GetAll()
        {
            return _orderRepository.GetAll().OrderByDescending(x=>x.CreatedDate);
        }

        public IEnumerable<Order> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _orderRepository.GetMulti(x => x.CustomerName.Contains(keyword) || x.CustomerEmail.Contains(keyword))
                    .OrderByDescending(x => x.CreatedDate);
            else
                return _orderRepository.GetAll();
        }

        public Order GetById(int id)
        {
            var order = _orderRepository.GetSingleById(id);
            var details = _orderDetailRepository.GetMulti(x => x.OrderId == id);
            order.OrderDetails = details;
            return order;
        }

        public IEnumerable<Order> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _orderRepository.GetAll().OrderByDescending(x=>x.CreatedDate);

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public void Update(Order order)
        {
            _orderRepository.Update(order);         
        }

        public Order Delete(int id)
        {
            var orderDetails = _orderDetailRepository.GetMulti(x => x.OrderId == id);
            foreach (var item in orderDetails)
            {
                _orderDetailRepository.Delete(item);
            }
            return _orderRepository.Delete(id);
        }
    }
}
