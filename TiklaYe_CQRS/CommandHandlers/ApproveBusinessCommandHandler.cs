using MediatR;  // MediatR kütüphanesi, CQRS patterninde kullanılan istek ve işlem sınıflarını yönetmek için kullanılır.
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Services;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class ApproveBusinessCommandHandler : IRequestHandler<ApproveBusinessCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public ApproveBusinessCommandHandler(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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

                // E-posta gönderme işlemi
                var emailSubject = "Hesabınız Onaylandı!";
                var emailBody = "Hesabınız TıklaYe tarafından onaylanmıştır! Aramıza hoş geldiniz. Sistemimize giriş yapabilirsiniz.";
                await _emailService.SendEmailAsync(businessOwner.Email, emailSubject, emailBody);

                return true;
            }
            return false;
        }
    }
}