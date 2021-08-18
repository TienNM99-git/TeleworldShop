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
        void UpdateStatus(int orderId, string orderStatus);
        void Save();
        IEnumerable<Order> GetAll();
        void Update(Order product);
        void RollBackOrder(int orderId);
        Order Delete(int id);
        IEnumerable<Order> GetAll(string keyword);
        Order GetById(int id);
        List<Product> GetOrderedProducts(int orderId);
        IEnumerable<Order> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        List<PurchaseHistoryViewModel> GetOrdersByUserId(string userId);
    }
    public class OrderService : IOrderService
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;
        IProductRepository _productRepository;
        IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._productRepository = productRepository;
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
                throw ex;
            }
        }

        public void UpdateStatus(int orderId, string orderStatus)
        {
            var order = _orderRepository.GetSingleById(orderId);
            order.Status = true;
            order.PaymentStatus = orderStatus =="Verified" ? "Paid" : "Unpaid";
            order.OrderStatus = orderStatus;
            _orderRepository.Update(order);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Order> GetAll()
        {
            return _orderRepository.GetAll().OrderByDescending(x => x.CreatedDate);
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
            var query = _orderRepository.GetAll().OrderByDescending(x => x.CreatedDate);

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

        public List<Product> GetOrderedProducts(int orderId)
        {
            List<Product> orderedDetails = new List<Product>();
            var detailList = _orderDetailRepository.GetMulti(x => x.OrderId == orderId).Select(ordered =>
                new { ordered.ProductId, ordered.Quantity, ordered.Price });
            for (int i = 0; i < detailList.Count(); i++)
            {
                orderedDetails.Add(_productRepository.GetSingleById(detailList.ElementAt(i).ProductId));
                orderedDetails.ElementAt(i).Quantity = detailList.ElementAt(i).Quantity;
                orderedDetails.ElementAt(i).Price = detailList.ElementAt(i).Price;
            }
            return orderedDetails;
        }

        public void RollBackOrder(int orderId)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            orderDetails = _orderDetailRepository.GetMulti(x => x.OrderId == orderId).ToList();
            foreach (OrderDetail orderDetail in orderDetails)
            {
                Product product = new Product();
                product = _productRepository.GetSingleById(orderDetail.ProductId);
                product.Quantity += orderDetail.Quantity;
                _productRepository.Update(product);
                _unitOfWork.Commit();
            }
        }

        public List<PurchaseHistoryViewModel> GetOrdersByUserId(string userId)
        {
            return _orderRepository.GetOrdersByUserId(userId);
        }
    }
}
