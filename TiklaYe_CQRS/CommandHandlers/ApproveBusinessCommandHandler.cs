using MediatR;  // MediatR kütüphanesi, CQRS patterninde kullanılan istek ve işlem sınıflarını yönetmek için kullanılır.
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class ApproveBusinessCommandHandler : IRequestHandler<ApproveBusinessCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public ApproveBusinessCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ApproveBusinessCommand request, CancellationToken cancellationToken)
        {
            // 'BusinessOwners' tablosunda 'BusinessOwnerId' ile eşleşen iş sahibi kaydını bulur.
            var businessOwner = await _context.BusinessOwners.FirstOrDefaultAsync(b => b.BusinessOwnerId == request.BusinessOwnerId);

            // Eğer iş sahibi kaydı bulunursa, onay işlemini gerçekleştirir.
            if (businessOwner != null)
            {
                businessOwner.IsApproved = true;
                businessOwner.IsActive = true; // İşletmeyi aktif hale getir.
                businessOwner.ApprovalDate = DateTime.UtcNow;
                _context.BusinessOwners.Update(businessOwner);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            return false;
        }
    }
}