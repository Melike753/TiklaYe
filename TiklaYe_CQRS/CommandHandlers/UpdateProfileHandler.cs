using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class UpdateProfileHandler
    {
        private readonly ApplicationDbContext _context;

        public UpdateProfileHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateProfileCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == command.Username);
            if (user == null)
            {
                return false;
            }

            user.Username = command.Username;
            user.Email = command.Email;
            user.Mobile = command.Mobile;
            user.Address = command.Address;

            if (!string.IsNullOrEmpty(command.Password))
            {
                user.Password = HashPassword(command.Password);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var builder = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}