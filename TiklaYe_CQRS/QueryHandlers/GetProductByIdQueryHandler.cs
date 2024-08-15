using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetProductByIdQueryHandler
    {
        private readonly ApplicationDbContext _context;

        public GetProductByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PartnerProduct> Handle(GetProductByIdQuery query)
        {
            return await _context.PartnerProducts.FindAsync(query.ProductId);
        }
    }
}