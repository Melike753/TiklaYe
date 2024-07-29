using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class LoginBusinessOwnerCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public LoginBusinessOwnerCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessOwner> Handle(LoginBusinessOwnerCommand command)
        {
            var businessOwner = await _context.BusinessOwners
                .FirstOrDefaultAsync(bo => bo.Email == command.Email);

            if (businessOwner == null || !VerifyPasswordHash(command.Password, businessOwner.PasswordHash))
            {
                return null; 
            }

            if (!businessOwner.IsApproved)
            {
                throw new Exception("Hesabınız henüz onaylanmamış.");
            }

            return businessOwner;
        }

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
                return builder.ToString() == storedHash;
            }
        }
    }
}