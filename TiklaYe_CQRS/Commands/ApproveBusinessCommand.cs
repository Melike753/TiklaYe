using MediatR;

namespace TiklaYe_CQRS.Commands
{
    public class ApproveBusinessCommand : IRequest<bool>
    {
        public int BusinessOwnerId { get; set; }
    }
}