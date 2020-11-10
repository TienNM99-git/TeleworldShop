using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeleworldShop.Model.Models;
using TeleworldShop.Service;
using TeleworldShop.Web.Mappings;
using TeleworldShop.Web.Models;

namespace TeleworldShop.Web.Controllers
{
    public class HomeController : Controller
    {
        IProductCategoryService _productCategoryService;
        ICommonService _commonService;
        public HomeController(IProductCategoryService productCategoryService, ICommonService commonService)
        {
            _productCategoryService = productCategoryService;
            _commonService = commonService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [ChildActionOnly]
        public ActionResult Footer()
        {
            var footerModel = _commonService.GetFooter();
            var mapper = new Mapper(AutoMapperConfiguration.Configure());
            var footerViewModel = mapper.Map<Footer,FooterViewModel>(footerModel);
            return PartialView(footerViewModel);
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Category()
        {
            var model = _productCategoryService.GetAll();
            var mapper = new Mapper(AutoMapperConfiguration.Configure());
            var listProductCategoryViewModel = mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
            return PartialView(listProductCategoryViewModel);
        }
    }
}