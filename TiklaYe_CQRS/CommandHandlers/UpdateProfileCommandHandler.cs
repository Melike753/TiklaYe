using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    // Bir kullanıcının profil bilgilerini güncellemek için kullanılır. 
    public class UpdateProfileCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public UpdateProfileCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateProfileCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Command cannot be null");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == command.Username);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (!string.IsNullOrWhiteSpace(command.Username))
            {
                user.Username = command.Username;
            }

            if (!string.IsNullOrWhiteSpace(command.Email))
            {
                user.Email = command.Email;
            }

            if (!string.IsNullOrWhiteSpace(command.Mobile))
            {
                user.Mobile = command.Mobile;
            }

            if (!string.IsNullOrWhiteSpace(command.Address))
            {
                user.Address = command.Address;
            }

            if (!string.IsNullOrEmpty(command.Password))
            {
                user.Password = HashPassword(command.Password);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
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