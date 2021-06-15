using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeleworldShop.Model.Models;
using TeleworldShop.Service;
using TeleworldShop.Web.Infrastructure.Core;
using TeleworldShop.Web.Models;
using TeleworldShop.Web.Infrastructure.Extensions;
using System.Web.Script.Serialization;
using TeleworldShop.Web.Mappings;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using OfficeOpenXml;
using TeleworldShop.Common;

namespace TeleworldShop.Web.Api
{
    [RoutePrefix("api/productcategory")]
    public class ProductCategoryController : ApiControllerBase
    {
        #region Initialize
        private IProductCategoryService _productCategoryService;

        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService)
            : base(errorService)
        {
            this._productCategoryService = productCategoryService;
        }

        #endregion
        [Authorize(Roles = "ViewCategory")]
        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productCategoryService.GetAll().Where(x=>x.Status).OrderByDescending(x=>x.Status).ThenByDescending(x => x.CreatedDate);

                var mapper = new Mapper(AutoMapperConfiguration.Configure());

                var responseData = mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }
        [Authorize(Roles = "ViewCategory")]
        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productCategoryService.GetById(id);

                var mapper = new Mapper(AutoMapperConfiguration.Configure());

                var responseData = mapper.Map<ProductCategory,ProductCategoryViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }
       
        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "ViewCategory")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _productCategoryService.GetAll(keyword).Where(x => x.Status);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.Status).ThenByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var mapper = new Mapper(AutoMapperConfiguration.Configure());

                var responseData = mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(query);

                var paginationSet = new PaginationSet<ProductCategoryViewModel>()
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
        [Route("getListAvailableCategory")]
        [HttpGet]
        public HttpResponseMessage GetCategoryByPromotionId(HttpRequestMessage request)
        {       
            var listCategory = _productCategoryService.GetListAvailableCategory();

            var mapper = new Mapper(AutoMapperConfiguration.Configure());

            var responseData = mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(listCategory);

            var response = request.CreateResponse(HttpStatusCode.OK, responseData);
            return response;

        }
        [Route("create")]
        [HttpPost]
        [Authorize(Roles = "AddCategory")]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductCategoryViewModel productCategoryVm)
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
                    var newProductCategory = new ProductCategory();
                    newProductCategory.UpdateProductCategory(productCategoryVm);
                    newProductCategory.CreatedDate = DateTime.Now;
                    newProductCategory.CreatedBy = User.Identity.Name;
                    _productCategoryService.Add(newProductCategory);
                    _productCategoryService.Save();

                    var mapper = new Mapper(AutoMapperConfiguration.Configure());

                    var responseData = mapper.Map<ProductCategory, ProductCategoryViewModel>(newProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [Authorize(Roles = "UpdateCategory")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductCategoryViewModel productCategoryVm)
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
                    var dbProductCategory = _productCategoryService.GetById(productCategoryVm.Id);

                    dbProductCategory.UpdateProductCategory(productCategoryVm);
                    dbProductCategory.UpdatedDate = DateTime.Now;
                    dbProductCategory.UpdatedBy = User.Identity.Name;
                    _productCategoryService.Update(dbProductCategory);
                    _productCategoryService.Save();

                    var mapper = new Mapper(AutoMapperConfiguration.Configure());

                    var responseData = mapper.Map<ProductCategory,ProductCategoryViewModel>(dbProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Authorize(Roles = "DeleteCategory")]
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
                    var oldProductCategory = _productCategoryService.GetById(id);
                    oldProductCategory.Status = false;
                    _productCategoryService.Update(oldProductCategory);
                    _productCategoryService.Save();
                    var mapper = new Mapper(AutoMapperConfiguration.Configure());

                    var responseData = mapper.Map<ProductCategory,ProductCategoryViewModel>(oldProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [Authorize(Roles = "DeleteCategory")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProductCategories)
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
                    var listProductCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedProductCategories);
                    foreach (var item in listProductCategory)
                    {
                        var oldProductCategory = _productCategoryService.GetById(item);
                        oldProductCategory.Status = false;
                        _productCategoryService.Update(oldProductCategory);
                    }
                    _productCategoryService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listProductCategory.Count);
                }

                return response;
            });
        }
        [Route("import")]
        [HttpPost]
        public async Task<HttpResponseMessage> Import()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Not supported format");
            }

            var root = HttpContext.Current.Server.MapPath("~/UploadedFiles/Excels");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var provider = new MultipartFormDataStreamProvider(root);
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            int? parentCategoryId = 0;
            //do stuff with files if you wish
            if (result.FormData["parentCategoryId"] == null)
            {
                parentCategoryId = null;
            }
            int pid = 0;
            int.TryParse(result.FormData["parentCategoryId"], out pid);
            parentCategoryId = (int)pid;    
            //Upload files
            int addedCount = 0;

            foreach (MultipartFileData fileData in result.FileData)
            {
                if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Request not in valid format");
                }
                string fileName = fileData.Headers.ContentDisposition.FileName;
                if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                {
                    fileName = fileName.Trim('"');
                }
                if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                {
                    fileName = Path.GetFileName(fileName);
                }

                var fullPath = Path.Combine(root, fileName);
                File.Copy(fileData.LocalFileName, fullPath, true);

                //insert to DB
                var listProductCategory = this.ReadCategoryFromExcel(fullPath, parentCategoryId);
                if (listProductCategory.Count > 0)
                {
                    foreach (var product in listProductCategory)
                    {
                        _productCategoryService.Add(product);
                        addedCount++;
                    }
                    _productCategoryService.Save();
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Imported successfully " + addedCount + " items.");
        }

        [HttpGet]
        [Route("ExportXls")]
        public async Task<HttpResponseMessage> ExportXls(HttpRequestMessage request, string filter = null)
        {
            string fileName = string.Concat("Product Categories_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
            var folderReport = ConfigHelper.GetByKey("ReportFolder");
            string filePath = HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                var data = _productCategoryService.GetListAvailableCategory().ToList();
                await ReportHelper.GenerateXls(data, fullPath);
                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        private List<ProductCategory> ReadCategoryFromExcel(string fullPath, int? parentCategoryId)
        {
            using (var package = new ExcelPackage(new FileInfo(fullPath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                List<ProductCategory> listProductCategory = new List<ProductCategory>();
                ProductCategoryViewModel productCategoryViewModel;
                ProductCategory category;

                bool status = false;
                bool showHome = false;
                int displayOrder;

                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    productCategoryViewModel = new ProductCategoryViewModel();
                    category = new ProductCategory();

                    productCategoryViewModel.Name = workSheet.Cells[i, 1].Value.ToString();
                    productCategoryViewModel.Alias = StringHelper.ToUnsignString(productCategoryViewModel.Name);
                    productCategoryViewModel.Description = workSheet.Cells[i, 2].Value.ToString();

                    if (int.TryParse(workSheet.Cells[i, 3].Value.ToString(), out displayOrder))
                    {
                        productCategoryViewModel.DisplayOrder = displayOrder;

                    }
                    productCategoryViewModel.ParentId = parentCategoryId;
                    productCategoryViewModel.CreatedDate = Convert.ToDateTime(DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                    productCategoryViewModel.UpdatedDate = Convert.ToDateTime(DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                    productCategoryViewModel.CreatedBy = "admin";
                    productCategoryViewModel.UpdatedBy = "admin";

                    bool.TryParse(workSheet.Cells[i, 4].Value.ToString(), out showHome);
                    productCategoryViewModel.HomeFlag = showHome;
                    bool.TryParse(workSheet.Cells[i, 5].Value.ToString(), out status);
                    productCategoryViewModel.Status = status;

                    category.UpdateProductCategory(productCategoryViewModel);
                    listProductCategory.Add(category);
                }
                return listProductCategory;
            }
        }
    }
}