using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TiklaYe.Models
{
    public class BusinessOwner
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad gereklidir.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad gereklidir.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-Posta gereklidir.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon Numarası gereklidir.")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        [MaxLength(256)]
        public string Password { get; set; }
    }
}
