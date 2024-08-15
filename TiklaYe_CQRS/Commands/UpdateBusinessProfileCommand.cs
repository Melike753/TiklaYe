using MediatR;

namespace TiklaYe_CQRS.Commands
{
    public class UpdateBusinessProfileCommand : IRequest<UpdateBusinessProfileResult>
    {
        public int BusinessOwnerId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}