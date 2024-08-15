using MediatR;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UpdateUserProfileResult>
    {
        private readonly ApplicationDbContext _context;

        public UpdateUserProfileCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateUserProfileResult> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId);

            if (user == null)
            {
                return new UpdateUserProfileResult { IsSuccess = false, ErrorMessage = "Kullanıcı bulunamadı." };
            }

            // Eski şifre kontrolü
            var hashedOldPassword = PasswordHasher.HashPassword(request.OldPassword);
            if (user.Password != hashedOldPassword)
            {
                return new UpdateUserProfileResult { IsSuccess = false, ErrorMessage = "Eski şifre yanlış." };
            }

            // Kullanıcı bilgilerini güncelle
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

            // Şifre güncellemesi
            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                user.Password = PasswordHasher.HashPassword(request.NewPassword);
            }

            _context.Update(user);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                return new UpdateUserProfileResult { IsSuccess = true };
            }
            else
            {
                return new UpdateUserProfileResult { IsSuccess = false, ErrorMessage = "Bilgiler güncellenirken bir hata oluştu." };
            }
        }
    }
}