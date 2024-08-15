using MediatR;

namespace TiklaYe_CQRS.Commands
{
    public class RegisterCommand : IRequest<bool>
    {
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public string RestaurantName { get; set; }

       public IFormFile PdfFile { get; set; }
    }
}