using MediatR;

namespace TiklaYe_CQRS.Commands
{
    public class UpdateProfileCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
    }
}