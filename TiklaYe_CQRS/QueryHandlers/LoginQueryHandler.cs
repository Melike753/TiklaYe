using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    // MediatR kullanarak LoginQuery işlemlerini yöneten sınıf.
    public class LoginQueryHandler : IRequestHandler<LoginQuery, BusinessOwner>
    {
        private readonly ApplicationDbContext _context;

        public LoginQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        // LoginQuery'yi işleyen metot. Giriş işlemlerini gerçekleştirir.
        public async Task<BusinessOwner> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            // Belirtilen email adresine sahip işletmeci (business owner) veritabanında aranır.
            var businessOwner = await _context.BusinessOwners
                .FirstOrDefaultAsync(b => b.Email == request.Email, cancellationToken);

            // İş sahibi bulunduysa ve şifre doğrulanırsa
            if (businessOwner != null && VerifyPasswordHash(request.Password, businessOwner.PasswordHash))
            {
                if (!businessOwner.IsApproved)
                {
                    return null;
                }

                return businessOwner;
            }

            return null;
        }

        // Şifre hash'ini doğrulayan yardımcı metot.
        private bool VerifyPasswordHash(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                // Hesaplanan hash ile saklanan hash'i karşılaştırır.
                return builder.ToString() == storedHash;
            }
        }
    }
}