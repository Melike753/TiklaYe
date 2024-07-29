using Microsoft.AspNetCore.Mvc;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Queries;
using TiklaYe_CQRS.QueryHandlers;

namespace TiklaYe_CQRS.Controllers
{
    public class AdminController : Controller
    {
        private readonly GetSalesReportQueryHandler _getSalesReportHandler;
        private readonly GetPendingBusinessRequestsQueryHandler _getPendingBusinessRequestsHandler;
        private readonly ApproveBusinessCommandHandler _approveBusinessHandler;

        // Constructor
        public AdminController(
            GetSalesReportQueryHandler getSalesReportHandler,
            GetPendingBusinessRequestsQueryHandler getPendingBusinessRequestsHandler,
            ApproveBusinessCommandHandler approveBusinessHandler)
        {
            _getSalesReportHandler = getSalesReportHandler;
            _getPendingBusinessRequestsHandler = getPendingBusinessRequestsHandler;
            _approveBusinessHandler = approveBusinessHandler;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Satış raporlarını görüntüleme
        public async Task<IActionResult> SalesReport(DateTime? startDate, DateTime? endDate)
        {
            var query = new GetSalesReportQuery
            {
                StartDate = startDate,
                EndDate = endDate
            };

            var salesReport = await _getSalesReportHandler.Handle(query);

            ViewBag.TotalRevenue = salesReport.Sum(sr => sr.TotalAmount);

            return View(salesReport);
        }

        // Bekleyen işletme isteklerini görüntüleme
        public async Task<IActionResult> BusinessRequests()
        {
            var query = new GetPendingBusinessRequestsQuery();
            var pendingBusinesses = await _getPendingBusinessRequestsHandler.Handle(query);

            return View(pendingBusinesses);
        }

        [HttpPost]
        // İşletme isteklerini onaylar
        public async Task<IActionResult> ApproveBusiness(int id)
        {
            var command = new ApproveBusinessCommand
            {
                Id = id
            };

            await _approveBusinessHandler.Handle(command);

            return RedirectToAction("BusinessRequests");
        }
    }
}