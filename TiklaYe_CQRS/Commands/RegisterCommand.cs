using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TiklaYe_CQRS.Commands
{
    public class RegisterCommand : IRequest<bool>
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string RestaurantName { get; set; }

       public IFormFile PdfFile { get; set; }

    }
}