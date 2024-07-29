using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class DeleteUserHandler
    {
        private readonly ApplicationDbContext _context;

        public DeleteUserHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteUserCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == command.UserId);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}