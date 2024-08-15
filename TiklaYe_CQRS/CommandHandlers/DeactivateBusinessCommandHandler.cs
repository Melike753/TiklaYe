using MediatR;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Commands;
using Microsoft.EntityFrameworkCore;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class DeactivateBusinessCommandHandler : IRequestHandler<DeactivateBusinessCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeactivateBusinessCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeactivateBusinessCommand request, CancellationToken cancellationToken)
        {
            var businessOwner = await _context.BusinessOwners.FindAsync(request.BusinessOwnerId);
            if (businessOwner != null)
            {
                // İşletmeyi pasif hale getir
                businessOwner.IsActive = false; 
                _context.BusinessOwners.Update(businessOwner);

                // İşletmecinin ürünlerini de pasif hale getir
                var products = await _context.PartnerProducts
                    .Where(p => p.BusinessOwnerId == request.BusinessOwnerId)
                    .ToListAsync();

                foreach (var product in products)
                {
                    product.IsActive = false;
                    _context.Update(product);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }
    }
}