using MediatR;

namespace TiklaYe_CQRS.Commands
{
    public class DeactivateBusinessCommand : IRequest<bool>
    {
        public int BusinessOwnerId { get; set; }
    }
}