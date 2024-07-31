using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.CommandHandlers
{
    // Bir işletmeciyi (business owner) kayıt etmek için kullanılır. 
    public class RegisterBusinessOwnerCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public RegisterBusinessOwnerCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(RegisterBusinessOwnerCommand command)
        {
            if (await _context.BusinessOwners.AnyAsync(u => u.Email == command.Email)) // Veritabanında mevcut bir işletmecinin olup olmadığını kontrol eder.
            {
                throw new Exception("E-posta zaten kullanılıyor.");
            }

            var businessOwner = new BusinessOwner
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                PhoneNumber = command.PhoneNumber,
                Password = HashPassword(command.Password),
            };

            _context.BusinessOwners.Add(businessOwner);
            await _context.SaveChangesAsync();
        }

        // Kullanıcı şifresini SHA256 algoritması ile hashler.
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Şifrenin hash'ini hesapla
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Byte array'i string'e dönüştür
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}