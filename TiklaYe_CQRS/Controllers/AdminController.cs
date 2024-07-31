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
        private readonly GetFeedbacksQueryHandler _getFeedbacksQueryHandler;
        private readonly GetAllUsersQueryHandler _getAllUsersHandler;
        private readonly DeleteUserCommandHandler _deleteUserHandler;
        private readonly GetGroupedPurchasesQueryHandler _getGroupedPurchasesHandler;
        private readonly UpdateStatusCommandHandler _updateStatusHandler;

        // Constructor
        public AdminController(
            GetSalesReportQueryHandler getSalesReportHandler,
            GetPendingBusinessRequestsQueryHandler getPendingBusinessRequestsHandler,
            ApproveBusinessCommandHandler approveBusinessHandler,
            GetFeedbacksQueryHandler getFeedbacksQueryHandler,
            GetAllUsersQueryHandler getAllUsersHandler, 
            DeleteUserCommandHandler deleteUserHandler,
            GetGroupedPurchasesQueryHandler getGroupedPurchasesHandler, 
            UpdateStatusCommandHandler updateStatusHandler)
        {
            _getSalesReportHandler = getSalesReportHandler;
            _getPendingBusinessRequestsHandler = getPendingBusinessRequestsHandler;
            _approveBusinessHandler = approveBusinessHandler;
            _getFeedbacksQueryHandler = getFeedbacksQueryHandler;
            _getAllUsersHandler = getAllUsersHandler;
            _deleteUserHandler = deleteUserHandler;
            _updateStatusHandler = updateStatusHandler;
            _getGroupedPurchasesHandler = getGroupedPurchasesHandler;
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

        // Geri bildirimleri görüntülemek için
        public async Task<IActionResult> AdminFeedback()
        {
            var query = new GetFeedbacksQuery();
            var feedbacks = await _getFeedbacksQueryHandler.Handle(query);
            return View(feedbacks);
        }

        public async Task<IActionResult> User()
        {
            var query = new GetAllUsersQuery();
            var users = await _getAllUsersHandler.Handle(query);
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteUserCommand { UserId = id };
            var success = await _deleteUserHandler.Handle(command);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Status()
        {
            var query = new GetGroupedPurchasesQuery();
            var groupedPurchases = await _getGroupedPurchasesHandler.Handle(query);
            return View(groupedPurchases);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string orderNumber, string status)
        {
            var command = new UpdateStatusCommand
            {
                OrderNumber = orderNumber,
                Status = status
            };

            var success = await _updateStatusHandler.Handle(command);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}