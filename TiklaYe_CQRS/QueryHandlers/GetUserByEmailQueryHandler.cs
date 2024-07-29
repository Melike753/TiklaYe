using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetUserByEmailQueryHandler
    {
        private readonly ApplicationDbContext _context;

        public GetUserByEmailQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> Handle(GetUserByEmailQuery query)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == query.Email);
        }
    }
}
