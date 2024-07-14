using System.ComponentModel.DataAnnotations;

namespace TiklaYe.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-Posta gereklidir.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}