using MediatR;
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ProfileViewModel>
    {
        private readonly ApplicationDbContext _context;

        public GetProfileQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileViewModel> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

            if (user == null)
            {
                return null;
            }

            return new ProfileViewModel
            {
                Username = user.Username,
                Email = user.Email,
                Mobile = user.Mobile,
                Address = user.Address,
                Password = string.Empty // Şifreyi göstermiyoruz.
            };
        }
    }
}