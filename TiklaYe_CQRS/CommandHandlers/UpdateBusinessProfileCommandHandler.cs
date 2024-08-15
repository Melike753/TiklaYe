using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class UpdateBusinessProfileCommandHandler : IRequestHandler<UpdateBusinessProfileCommand, UpdateBusinessProfileResult>
    {
        private readonly ApplicationDbContext _context;

        public UpdateBusinessProfileCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateBusinessProfileResult> Handle(UpdateBusinessProfileCommand request, CancellationToken cancellationToken)
        {
            var businessOwner = await _context.BusinessOwners.FindAsync(request.BusinessOwnerId);

            if (businessOwner == null)
            {
                return new UpdateBusinessProfileResult { IsSuccess = false, ErrorMessage = "İşletmeci bulunamadı." };
            }

            // Eski şifre kontrolü
            var hashedOldPassword = PasswordHasher.HashPassword(request.OldPassword);
            if (businessOwner.PasswordHash != hashedOldPassword)
            {
                return new UpdateBusinessProfileResult { IsSuccess = false, ErrorMessage = "Eski şifre yanlış." };
            }

            // E-posta ve telefon numarası güncellemesi
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                businessOwner.Email = request.Email;
            }

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                businessOwner.PhoneNumber = request.PhoneNumber;
            }

            // Şifre güncellemesi
            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                businessOwner.PasswordHash = PasswordHasher.HashPassword(request.NewPassword);
            }

            _context.Update(businessOwner);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                return new UpdateBusinessProfileResult { IsSuccess = true };
            }
            else
            {
                return new UpdateBusinessProfileResult { IsSuccess = false, ErrorMessage = "Bilgiler güncellenirken bir hata oluştu." };
            }
        }
    }
}