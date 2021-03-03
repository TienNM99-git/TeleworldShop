using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
        [Authorize(Roles = "ViewProduct")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _productService.GetAll().Where(x=>x.Status==true).OrderByDescending(x => x.CreatedDate);
                var mapper = new Mapper(AutoMapperConfiguration.Configure());

                var responseData = mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            };
            return CreateHttpResponse(request, func);
        }
        [Route("getbyid/{id:int}")]
        [Authorize(Roles = "ViewProduct")]
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
        [Authorize(Roles = "ViewProduct")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _productService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.Where(x => x.Status==true).OrderByDescending(x=>x.CreatedDate).Skip(page * pageSize).Take(pageSize);
                
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
        [Authorize(Roles = "AddProduct")]
        [HttpPost]
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
        [Authorize(Roles = "UpdateProduct")]
        [HttpPut]
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
        [Authorize(Roles = "DeleteProduct")]
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
        [Authorize(Roles = "DeleteProduct")]
        [HttpDelete]
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
        [Route("export")]
        [HttpPost]
        public HttpResponseMessage Export([FromBody] int productId)
        {
            var productData = _productService.GetAll();

            List<ExportProductViewModel> productVMs = new List<ExportProductViewModel>();

            foreach (var product in productData)
            {
                ExportProductViewModel p = new ExportProductViewModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Alias = product.Alias,
                    CategoryId = product.CategoryId,
                    Content = product.Content,
                    Image = product.Image,
                    MoreImages = product.MoreImages,
                    Price = product.Price,
                    PromotionPrice = product.PromotionPrice,
                    Warranty = product.Warranty,
                    HomeFlag = product.HomeFlag,
                    HotFlag = product.HotFlag,
                    ViewCount = product.ViewCount,

                    CreatedDate = product.CreatedDate,
                    CreatedBy = product.CreatedBy,
                    UpdatedDate = product.UpdatedDate,
                    UpdatedBy = product.UpdatedBy,
                    MetaKeyword = product.MetaKeyword,
                    MetaDescription = product.MetaDescription,
                    Status = product.Status,
                    Tags = product.Tags,
                    Quantity = product.Quantity,
                    OriginalPrice = product.OriginalPrice
                };
                productVMs.Add(p);
            }
            DataTable pTable = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(productVMs), (typeof(DataTable)));

            var now = DateTime.Now.ToString();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create($"D:/{Guid.NewGuid()}.xlsx", SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };

                sheets.Append(sheet);

                //p header
                Row pHeaderRow = new Row();

                List<string> pColumns = new List<string>();
                foreach (DataColumn column in pTable.Columns)
                {
                    pColumns.Add(column.ColumnName);

                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(column.ColumnName);
                    pHeaderRow.AppendChild(cell);
                }
                sheetData.AppendChild(pHeaderRow);
             
                //Write Data
                foreach (DataRow pRow in pTable.Rows)
                {
                    Row newRow = new Row();
                    foreach (String pCol in pColumns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(pRow[pCol].ToString());
                        newRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(newRow);                  
                }

                workbookPart.Workbook.Save();
            }
            return null;
        }

    }
}
