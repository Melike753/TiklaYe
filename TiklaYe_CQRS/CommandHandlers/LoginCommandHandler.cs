using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class LoginCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public LoginCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> Handle(LoginCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == command.Email);
            if (user != null && VerifyPasswordHash(command.Password, user.Password))
            {
                return user;
            }
            return null;
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