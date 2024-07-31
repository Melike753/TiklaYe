using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.CommandHandlers
{
    // Kullanıcıların geri bildirimlerini veritabanına kaydetmek için kullanılır. 
    public class CreateFeedbackCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public CreateFeedbackCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(CreateFeedbackCommand command)
        {
            var feedback = new Feedback
            {
                UserId = command.UserId,
                Name = command.Name,
                Email = command.Email,
                Subject = command.Subject,
                Message = command.Message
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
        }
    }
}