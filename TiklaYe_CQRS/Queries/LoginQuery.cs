using MediatR;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Queries
{
    public class LoginQuery : IRequest<BusinessOwner>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}