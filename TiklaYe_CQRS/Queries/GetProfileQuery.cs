using MediatR;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Queries
{
    public class GetProfileQuery : IRequest<ProfileViewModel>
    {
        public int UserId { get; set; }

        public GetProfileQuery(int userId)
        {
            UserId = userId;
        }
    }
}