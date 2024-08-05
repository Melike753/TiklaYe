using MediatR;

namespace TiklaYe_CQRS.Commands
{
    public class ActivateBusinessCommand : IRequest<bool>
    {
        public int BusinessOwnerId { get; set; }
    }
}