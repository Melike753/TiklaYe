using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetUserPurchasesHandler
    {
        private readonly ApplicationDbContext _context;

        public GetUserPurchasesHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Purchase>> Handle(GetUserPurchasesQuery query)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == query.UserName);
            if (user == null)
                return new List<Purchase>();

            return await _context.Purchases.Where(p => p.UserId == user.UserId).ToListAsync();
        }
    }
}