using MediatR;
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetBusinessProfileQueryHandler : IRequestHandler<GetBusinessProfileQuery, BusinessProfileViewModel>
    {
        private readonly ApplicationDbContext _context;

        public GetBusinessProfileQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessProfileViewModel> Handle(GetBusinessProfileQuery request, CancellationToken cancellationToken)
        {
            var businessOwner = await _context.BusinessOwners
               .AsNoTracking()
               .FirstOrDefaultAsync(b => b.BusinessOwnerId == request.BusinessOwnerId, cancellationToken);

            if (businessOwner == null)
            {
                return null;
            }

            return new BusinessProfileViewModel
            {
                BusinessOwnerId = businessOwner.BusinessOwnerId,
                Email = businessOwner.Email,
                PhoneNumber = businessOwner.PhoneNumber,
            };
        }
    }
}