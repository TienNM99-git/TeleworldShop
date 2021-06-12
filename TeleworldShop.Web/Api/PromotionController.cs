using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using TeleworldShop.Common.Exceptions;
using TeleworldShop.Model.Models;
using TeleworldShop.Service;
using TeleworldShop.Web.Infrastructure.Core;
using TeleworldShop.Web.Infrastructure.Extensions;
using TeleworldShop.Web.Mappings;
using TeleworldShop.Web.Models;

namespace TeleworldShop.Web.Api
{
    [RoutePrefix("api/promotion")]
    [Authorize(Roles = "Admin")]
    public class PromotionController : ApiControllerBase
    {

        #region Initialize
        private IPromotionService _promotionService;
        private IProductCategoryService _productCategoryService;

        public PromotionController(IErrorService errorService, IPromotionService promotionService, IProductCategoryService productCategoryService)
            : base(errorService)
        {
            this._promotionService = promotionService;
            this._productCategoryService = productCategoryService;
        }
        #endregion

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, PromotionViewModel promotionVm)
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
                    Promotion newPromotion = new Promotion();
                    newPromotion.UpdatePromotion(promotionVm);
                    newPromotion.CreatedBy = User.Identity.Name;
                    newPromotion.StartDate = promotionVm.StartDate.ToLocalTime();
                    newPromotion.ExpireDate = promotionVm.ExpireDate.ToLocalTime();
                    newPromotion.CreatedDate = DateTime.Now;

                    var promotion = _promotionService.Add(newPromotion);
                    _promotionService.Save();

                    var promotionDetails = new List<PromotionDetail>();

                    if (promotion.Apply == 1)
                    {
                        foreach (var category in promotionVm.Categories)
                        {
                            promotionDetails.Add(new PromotionDetail()
                            {
                                PromotionId = promotion.Id,
                                CategoryId = category.Id,
                            });
                        }
                    }
                    else
                    {
                        foreach (var tag in promotionVm.Tags)
                        {
                            promotionDetails.Add(new PromotionDetail()
                            {
                                PromotionId = promotion.Id,
                                CategoryId = 0,
                            });
                        }
                    }

                    _promotionService.AddPromotionDetail(promotionDetails, promotion.Id);
                    _promotionService.Save();

                    _promotionService.UpdateProductPromotionPrice(promotion, promotionDetails);
                    _promotionService.Save();

                    var mapper = new Mapper(AutoMapperConfiguration.Configure());

                    var responseData = mapper.Map<Promotion, PromotionViewModel>(newPromotion);

                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }
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
                var model = _promotionService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var mapper = new Mapper(AutoMapperConfiguration.Configure());

                var responseData = mapper.Map<IEnumerable<Promotion>, IEnumerable<PromotionViewModel>>(query.AsEnumerable());

                var paginationSet = new PaginationSet<PromotionViewModel>()
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

        [Route("detail/{id:int}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, int id)
        {
            if (id == 0)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " is required.");
            }
            Promotion promotion = _promotionService.GetById(id);
            var mapper = new Mapper(AutoMapperConfiguration.Configure());
            var promotionVm = mapper.Map<Promotion, PromotionViewModel>(promotion);

            if (promotion == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "No promotion");
            }

            var listCategory = _productCategoryService.GetListCategoryByPromotionId(promotionVm.Id);
            promotionVm.Categories = mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(listCategory);

            return request.CreateResponse(HttpStatusCode.OK, promotionVm);
        }

        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, PromotionViewModel promotionVm)
        {
            if (ModelState.IsValid)
            {
                var promotion = _promotionService.GetById(promotionVm.Id);
                try
                {
                    promotion.UpdatePromotion(promotionVm);
                    promotion.UpdatedBy = User.Identity.Name;
                    promotion.StartDate = promotionVm.StartDate.ToLocalTime();
                    promotion.ExpireDate = promotionVm.ExpireDate.ToLocalTime();
                    _promotionService.Update(promotion);

                    var promotionDetails = new List<PromotionDetail>();

                    foreach (var category in promotionVm.Categories)
                    {
                        promotionDetails.Add(new PromotionDetail()
                        {
                            PromotionId = promotion.Id,
                            CategoryId = category.Id,
                        });
                    }

                    _promotionService.AddPromotionDetail(promotionDetails, promotion.Id);
                    _promotionService.Save();

                    _promotionService.UpdateProductPromotionPrice(promotion, promotionDetails);
                    _promotionService.Save();

                    var mapper = new Mapper(AutoMapperConfiguration.Configure());

                    var responseData = mapper.Map<Promotion, PromotionViewModel>(promotion);

                    return request.CreateResponse(HttpStatusCode.OK, responseData);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("delete")]
        [HttpDelete]
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
                    var promotion = _promotionService.GetById(id);
                    promotion.Status = false;
                    _promotionService.Update(promotion);
                    _promotionService.Save();

                    _promotionService.UpdateProductPromotionPrice(id);
                    _promotionService.Save();

                    var mapper = new Mapper(AutoMapperConfiguration.Configure());

                    var responseData = mapper.Map<Promotion, PromotionViewModel>(promotion);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedPromotions)
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
                    var listPromotion = new JavaScriptSerializer().Deserialize<List<int>>(checkedPromotions);
                    foreach (var item in listPromotion)
                    {
                        var promotion = _promotionService.GetById(item);
                        promotion.Status = false;
                        _promotionService.Update(promotion);

                        _promotionService.UpdateProductPromotionPrice(item);
                    }

                    _promotionService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listPromotion.Count);
                }

                return response;
            });
        }
    }
}