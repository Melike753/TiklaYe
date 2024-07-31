using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class LogoutCommandHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogoutCommandHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(Commands.LogoutCommand command)
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}