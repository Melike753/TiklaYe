using MediatR;
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class ActivateBusinessCommandHandler : IRequestHandler<ActivateBusinessCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public ActivateBusinessCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ActivateBusinessCommand request, CancellationToken cancellationToken)
        {
            var business = await _context.BusinessOwners.FindAsync(request.BusinessOwnerId);
            if (business == null)
            {
                return false;
            }

            business.IsActive = true;
            _context.BusinessOwners.Update(business);

            // İşletmecinin ürünlerini de aktif hale getir
            var products = await _context.PartnerProducts
                .Where(p => p.BusinessOwnerId == request.BusinessOwnerId)
                .ToListAsync();

            foreach (var product in products)
            {
                product.IsActive = true;
                _context.Update(product);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}