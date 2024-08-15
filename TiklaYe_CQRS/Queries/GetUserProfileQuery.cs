using MediatR;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Queries
{
    public class GetUserProfileQuery : IRequest<UserProfileViewModel>
    {
        public int UserId { get; set; }

        public GetUserProfileQuery(int userId)
        {
            UserId = userId;
        }
    }
}