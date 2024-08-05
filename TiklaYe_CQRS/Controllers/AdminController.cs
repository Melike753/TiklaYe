using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Queries;
using TiklaYe_CQRS.QueryHandlers;

namespace TiklaYe_CQRS.Controllers
{
    // AdminController sınıfı, yönetici işlemlerini gerçekleştirmek için kullanılır.
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
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        // Constructor
        public AdminController(
            GetSalesReportQueryHandler getSalesReportHandler,
            GetPendingBusinessRequestsQueryHandler getPendingBusinessRequestsHandler,
            ApproveBusinessCommandHandler approveBusinessHandler,
            GetFeedbacksQueryHandler getFeedbacksQueryHandler,
            GetAllUsersQueryHandler getAllUsersHandler, 
            DeleteUserCommandHandler deleteUserHandler,
            GetGroupedPurchasesQueryHandler getGroupedPurchasesHandler, 
            UpdateStatusCommandHandler updateStatusHandler,
            ApplicationDbContext context, 
            IMediator mediator)
        {
            _getSalesReportHandler = getSalesReportHandler;
            _getPendingBusinessRequestsHandler = getPendingBusinessRequestsHandler;
            _approveBusinessHandler = approveBusinessHandler;
            _getFeedbacksQueryHandler = getFeedbacksQueryHandler;
            _getAllUsersHandler = getAllUsersHandler;
            _deleteUserHandler = deleteUserHandler;
            _updateStatusHandler = updateStatusHandler;
            _getGroupedPurchasesHandler = getGroupedPurchasesHandler;
            _context = context;
            _mediator = mediator;
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

        // İşletme taleplerini görüntüleme
        [HttpGet]
        public async Task<IActionResult> BusinessRequests()
        {
            // Onaylanmamış işletme taleplerini al
            var businessRequests = await _context.BusinessOwners
                .Where(b => !b.IsApproved)
                .ToListAsync();
            return View(businessRequests);
        }

        // İşletme talebini onaylama
        [HttpPost]
        public async Task<IActionResult> ApproveBusiness(int id)
        {
            var command = new ApproveBusinessCommand
            {
                BusinessOwnerId = id
            };

            bool result = await _mediator.Send(command);
            if (result)
            {
                return RedirectToAction("BusinessRequests");
            }

            return View("Error");
        }

        // Geri bildirimleri görüntülemek için
        public async Task<IActionResult> AdminFeedback()
        {
            var query = new GetFeedbacksQuery();
            var feedbacks = await _getFeedbacksQueryHandler.Handle(query);
            return View(feedbacks);
        }

        // Kullanıcıları görüntüleme
        public async Task<IActionResult> User()
        {
            var query = new GetAllUsersQuery();
            var users = await _getAllUsersHandler.Handle(query);
            return View(users);
        }

        // Bir kullanıcıyı siler.
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

        // Sipariş Durum güncellemelerini görüntüleme
        public async Task<IActionResult> Status()
        {
            var query = new GetGroupedPurchasesQuery();
            var groupedPurchases = await _getGroupedPurchasesHandler.Handle(query);
            return View(groupedPurchases);
        }

        // Bir siparişin durumunu günceller.
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

        [HttpPost]
        public async Task<IActionResult> DeactivateBusiness(int id)
        {
            var command = new DeactivateBusinessCommand
            {
                BusinessOwnerId = id
            };

            bool result = await _mediator.Send(command);
            if (result)
            {
                return RedirectToAction("ViewBusinesses"); // İşletmelerin görüntülendiği sayfaya yönlendirir.
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> ActivateBusiness(int id)
        {
            var command = new ActivateBusinessCommand
            {
                BusinessOwnerId = id
            };

            bool result = await _mediator.Send(command);
            if (result)
            {
                return RedirectToAction("ViewBusinesses"); // İşletmelerin görüntülendiği sayfaya yönlendirir.
            }

            return View("Error");
        }


        // Onaylanan işletmecileri görüntüleme
        public async Task<IActionResult> ViewBusinesses()
        {
            var businesses = await _context.BusinessOwners
                .Where(b => b.IsApproved)
                .ToListAsync();

            return View(businesses);
        }
    }
}