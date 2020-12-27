using AutoMapper;
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
    [RoutePrefix("api/product")]
    [Authorize(Roles ="Admin")]
    public class ProductController : ApiControllerBase
    {
        #region Initialize
        private IProductService _productService;

        public ProductController(IErrorService errorService, IProductService productService)
            : base(errorService)
        {
            this._productService = productService;
        }

        #endregion

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _productService.GetAll().OrderByDescending(x=>x.Status).ThenByDescending(x => x.CreatedDate);
                var mapper = new Mapper(AutoMapperConfiguration.Configure());

                var responseData = mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            };
            return CreateHttpResponse(request, func);
        }
        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productService.GetById(id);

                var mapper = new Mapper(AutoMapperConfiguration.Configure());

                var responseData = mapper.Map<Product, ProductViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _productService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.Status).ThenByDescending(x=>x.CreatedDate).Skip(page * pageSize).Take(pageSize);
                
                var mapper = new Mapper(AutoMapperConfiguration.Configure());

                var responseData = mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(query.AsEnumerable());

                var paginationSet = new PaginationSet<ProductViewModel>()
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


        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel productVm)
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
                    var newProduct = new Product();
                    newProduct.UpdateProduct(productVm);
                    newProduct.CreatedDate = DateTime.Now;
                    newProduct.CreatedBy = User.Identity.Name;
                    _productService.Add(newProduct);
                    _productService.Save();

                    var mapper = new Mapper(AutoMapperConfiguration.Configure());

                    var responseData = mapper.Map<Product, ProductViewModel>(newProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel productVm)
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
                    var dbProduct = _productService.GetById(productVm.Id);

                    dbProduct.UpdateProduct(productVm);
                    dbProduct.UpdatedDate = DateTime.Now;
                    dbProduct.UpdatedBy = User.Identity.Name;
                    _productService.Update(dbProduct);
                    _productService.Save();

                    var mapper = new Mapper(AutoMapperConfiguration.Configure());
                    var responseData = mapper.Map<Product, ProductViewModel>(dbProduct);
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
                    var oldProduct = _productService.GetById(id);
                    oldProduct.Status = false;
                    _productService.Update(oldProduct);
                    _productService.Save();

                    var mapper = new Mapper(AutoMapperConfiguration.Configure());

                    var responseData = mapper.Map<Product, ProductViewModel>(oldProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProducts)
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
                    var listProduct = new JavaScriptSerializer().Deserialize<List<int>>(checkedProducts);
                    foreach (var item in listProduct)
                    {
                        var oldProduct = _productService.GetById(item);
                        oldProduct.Status = false;
                        _productService.Update(oldProduct);
                    }

                    _productService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listProduct.Count);
                }

                return response;
            });
        }
    }
}
