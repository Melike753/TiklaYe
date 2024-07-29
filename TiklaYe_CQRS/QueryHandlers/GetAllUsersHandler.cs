using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetAllUsersHandler
    {
        private readonly ApplicationDbContext _context;

        public GetAllUsersHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> Handle(GetAllUsersQuery query)
        {
            return await _context.Users.ToListAsync();
        }
    }
}