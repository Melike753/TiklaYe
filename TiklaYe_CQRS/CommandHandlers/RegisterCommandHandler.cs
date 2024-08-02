using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        // Constructor dependency injection
        public RegisterCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        // RegisterCommand'ın işlenmesi
        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // E-posta adresinin zaten var olup olmadığını kontrol eder.
            if (await _context.BusinessOwners.AnyAsync(u => u.Email == request.Email, cancellationToken))
            {
                return false; // E-posta adresi zaten kullanılıyorsa false döner.
            }

            // Yeni işletmeci oluşturur.
            var businessOwner = new BusinessOwner
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = PasswordHasher.HashPassword(request.Password), // Parolayı hash'le
                RestaurantName = request.RestaurantName,
                IsApproved = false // Varsayılan olarak onaylanmamış
            };

            // Yeni işletmeciyi veritabanına ekler
            _context.BusinessOwners.Add(businessOwner);
            await _context.SaveChangesAsync(cancellationToken); // Değişiklikleri kaydeder
            return true; // İşlem başarılıysa true döner
        } 
    }

    // Parola hashleme işlemleri için yardımcı sınıf
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // SHA256 algoritması kullanarak parolayı hash'ler
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));  // Her byte'ı iki basamaklı hexadecimal olarak ekler.
                }
                return builder.ToString();  // Hash'lenmiş parolayı string olarak döner.
            }
        }
    }
}