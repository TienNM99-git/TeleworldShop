using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeleworldShop.Service;
using TeleworldShop.Web.Infrastructure.Core;

namespace TeleworldShop.Web.Api
{
    [Authorize(Roles = "ViewStatistics")]
    [RoutePrefix("api/statistic")]
    public class StatisticController : ApiControllerBase
    {
        private IStatisticService _statisticService;

        public StatisticController(IErrorService errorService, IStatisticService statisticService) : base(errorService)
        {
            _statisticService = statisticService;
        }

        [Route("getrevenue")]
        [HttpGet]
        public HttpResponseMessage GetRevenueStatistic(HttpRequestMessage request, string fromDate, string toDate)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _statisticService.GetRevenueStatistic(fromDate, toDate);
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

        [Route("getorderstatistics")]
        [HttpGet]
        public HttpResponseMessage GetOrderStatistic(HttpRequestMessage request, string fromDate, string toDate)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _statisticService.GetOrderStatistic(fromDate, toDate);
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

        [Route("getsellstatistics")]
        [HttpGet]
        public HttpResponseMessage GetSellStatistic(HttpRequestMessage request, string fromDate, string toDate)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _statisticService.GetSellStatistic(fromDate, toDate);
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

        [Route("getinventorystatistics")]
        [HttpGet]
        public HttpResponseMessage GetInventoryStatistic(HttpRequestMessage request, string fromDate, string toDate)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _statisticService.GetInventoryStatistic(fromDate, toDate);
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }
    }
}