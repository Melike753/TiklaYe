using System.ComponentModel.DataAnnotations;

namespace TiklaYe_CQRS.Models
{
    public class RegisterViewModel
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Ad Soyad gereklidir.")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Kullanıcı Adı gereklidir.")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "E-Posta gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon Numarası gereklidir.")]
        [Phone]
        [StringLength(50)]
        public string Mobile { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        [MaxLength(256)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre Onayı gereklidir.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        [MaxLength(256)]
        public string ConfirmPassword { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}