using System.Web.Http;
using TeleworldShop.Service;
using TeleworldShop.Web.Infrastructure.Core;

namespace TeleworldShop.Web.Api
{
    [RoutePrefix("api/home")]
    [Authorize]
    public class HomeController : ApiControllerBase
    {
        private IErrorService _errorService;

        public HomeController(IErrorService errorService) : base(errorService)
        {
            this._errorService = errorService;
        }

        [HttpGet]
        [Route("TestMethod")]
        public string TestMethod()
        {
            return "Hello, This is your Admin-TienVipPro ";
        }
    }
}