using Microsoft.AspNetCore.Mvc;
using TiklaYe_CQRS.Queries;
using TiklaYe_CQRS.QueryHandlers;

// Geri bildirimleri görüntülemek için
namespace TiklaYe_CQRS.Controllers
{
    public class AdminFeedbackController : Controller
    {
        private readonly GetFeedbacksQueryHandler _getFeedbacksQueryHandler;

        // Constructor
        public AdminFeedbackController(GetFeedbacksQueryHandler getFeedbacksQueryHandler)
        {
            _getFeedbacksQueryHandler = getFeedbacksQueryHandler;
        }

        public async Task<IActionResult> Index()
        {
            var query = new GetFeedbacksQuery();
            var feedbacks = await _getFeedbacksQueryHandler.Handle(query);
            return View(feedbacks);
        }
    }
}