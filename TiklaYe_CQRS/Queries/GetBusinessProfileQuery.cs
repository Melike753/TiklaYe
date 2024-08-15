using MediatR;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Queries
{
    public class GetBusinessProfileQuery : IRequest<BusinessProfileViewModel>
    {
        public int BusinessOwnerId { get; set; }

        public GetBusinessProfileQuery(int businessOwnerId)
        {
            BusinessOwnerId = businessOwnerId;
        }
    }
}