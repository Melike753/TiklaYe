using System.ComponentModel.DataAnnotations;

namespace TiklaYe_CQRS.Models
{
    public class BusinessLoginViewModel
    {
        [Required(ErrorMessage = "E-Posta gereklidir.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        [MaxLength(256)]
        public string Password { get; set; }
    }
}