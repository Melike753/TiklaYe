using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetUserProfileQueryHandler
    {
        private readonly ApplicationDbContext _context;

        public GetUserProfileQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> Handle(GetUserProfileQuery query)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == query.Username);
        }
    }
}