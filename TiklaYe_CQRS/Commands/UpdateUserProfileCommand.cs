using MediatR;

namespace TiklaYe_CQRS.Commands
{
    public class UpdateUserProfileCommand : IRequest<UpdateUserProfileResult>
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
    }
}