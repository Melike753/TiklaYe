using Microsoft.AspNetCore.Mvc;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.Controllers
{
    public class StatusController : Controller
    {
        private readonly GetGroupedPurchasesHandler _getGroupedPurchasesHandler;
        private readonly UpdateStatusHandler _updateStatusHandler;

        public StatusController(GetGroupedPurchasesHandler getGroupedPurchasesHandler, UpdateStatusHandler updateStatusHandler)
        {
            _getGroupedPurchasesHandler = getGroupedPurchasesHandler;
            _updateStatusHandler = updateStatusHandler;
        }

        public async Task<IActionResult> Index()
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