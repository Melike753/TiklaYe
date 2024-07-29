using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class ApproveBusinessCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public ApproveBusinessCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(ApproveBusinessCommand command)
        {
            var businessOwner = await _context.BusinessOwners.FindAsync(command.Id);
            if (businessOwner != null)
            {
                businessOwner.IsApproved = true;
                businessOwner.ApprovalDate = DateTime.Now;
                _context.Update(businessOwner);
                await _context.SaveChangesAsync();
            }
        }
    }
}