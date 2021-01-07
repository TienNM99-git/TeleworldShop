﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using TeleworldShop.Model.Models;
using TeleworldShop.Service;
using TeleworldShop.Web.Infrastructure.Core;
using TeleworldShop.Web.Infrastructure.Extensions;
using TeleworldShop.Web.Mappings;
using TeleworldShop.Web.Models;

namespace TeleworldShop.Web.Api
{
    [RoutePrefix("api/order")]
    //[Authorize(Roles ="Admin")]
    public class OrderController : ApiControllerBase
    {
        #region Initialize
        private IOrderService _orderService;
        private IProductService _productService;
        //private IOrderDetailService _orderDetailService;
        public OrderController(IErrorService errorService, IOrderService orderService, IProductService productService)
            : base(errorService)
        {
            this._orderService = orderService;
            this._productService = productService;
        }

        #endregion
        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _orderService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var mapper = new Mapper(AutoMapperConfiguration.Configure());

                var responseData = mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(query.AsEnumerable());

                var paginationSet = new PaginationSet<OrderViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _orderService.GetAll().OrderByDescending(x => x.CreatedDate);
                var mapper = new Mapper(AutoMapperConfiguration.Configure());

                var responseData = mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            };
            return CreateHttpResponse(request, func);
        }
        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetOrderDetails(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _orderService.GetById(id);

                var mapper = new Mapper(AutoMapperConfiguration.Configure());

                var responseData = mapper.Map<Order, OrderViewModel>(model);

              //  var responseData = mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailViewModel>>(order.OrderDetails);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }
        [Route("getorderedproduct/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetOrderedProduct(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _orderService.GetById(id);

                var mapper = new Mapper(AutoMapperConfiguration.Configure());

                var responseData = mapper.Map<Order, OrderViewModel>(model);

                //  var responseData = mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailViewModel>>(order.OrderDetails);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }
        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, OrderViewModel orderViewModel)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var dbOrder = _orderService.GetById(orderViewModel.Id);

                    dbOrder.UpdateOrder(orderViewModel);
                    _orderService.Update(dbOrder);
                    _orderService.Save();

                    var mapper = new Mapper(AutoMapperConfiguration.Configure());
                    var responseData = mapper.Map<Order, OrderViewModel>(dbOrder);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var oldOrder = _orderService.GetById(id);
                    _orderService.Delete(oldOrder.Id);
                    _orderService.Save();

                    var mapper = new Mapper(AutoMapperConfiguration.Configure());

                    var responseData = mapper.Map<Order, OrderViewModel>(oldOrder);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedOrders)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listOrder = new JavaScriptSerializer().Deserialize<List<int>>(checkedOrders);
                    foreach (var item in listOrder)
                    {
                        var oldOrder = _orderService.GetById(item);
                        _orderService.Delete(oldOrder.Id);
                    }

                    _orderService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listOrder.Count);
                }

                return response;
            });
        }
    }
}
