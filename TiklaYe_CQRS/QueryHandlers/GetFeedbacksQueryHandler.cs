using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetFeedbacksQueryHandler
    {
        private readonly ApplicationDbContext _context;

        public GetFeedbacksQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Feedback>> Handle(GetFeedbacksQuery query)
        {
            return await _context.Feedbacks
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync();
        }
    }
}