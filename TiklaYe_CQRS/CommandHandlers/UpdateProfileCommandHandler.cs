using MediatR;
using System.Security.Cryptography;
using System.Text;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    // Bir kullanıcının profil bilgilerini güncellemek için kullanılır. 
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public UpdateProfileCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId);

            if (user == null)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                user.Username = request.Username;
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                user.Email = request.Email;
            }

            if (!string.IsNullOrWhiteSpace(request.Mobile))
            {
                user.Mobile = request.Mobile;
            }

            if (!string.IsNullOrWhiteSpace(request.Address))
            {
                user.Address = request.Address;
            }

            if (!string.IsNullOrEmpty(request.Password))
            {
                user.Password = HashPassword(request.Password);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
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