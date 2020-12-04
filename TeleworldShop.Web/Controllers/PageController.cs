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
    public class PageController : Controller
    {
        IPageService _pageService;
        public PageController(IPageService pageService)
        {
            this._pageService = pageService;
        }
        // GET: Page
        public ActionResult Index(string alias)
        {
            var page = _pageService.GetByAlias(alias);
            var mapper = new Mapper(AutoMapperConfiguration.Configure());
            var model = mapper.Map<Page,PageViewModel>(page);
            return View(model);
        }
    }
}