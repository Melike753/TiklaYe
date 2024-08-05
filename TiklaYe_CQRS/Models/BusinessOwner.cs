using System.ComponentModel.DataAnnotations;

namespace TiklaYe_CQRS.Models
{
    public class BusinessOwner
    {
        [Key]
        public int BusinessOwnerId { get; set; }

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
        public string PasswordHash { get; set; }

        [Required]
        public string RestaurantName {  get; set; }

        public string LogoUrl => $"/images/logos/{BusinessOwnerId}.webp"; // İşletmeci logosu URL'i

        public bool IsApproved { get; set; }

        public DateTime? ApprovalDate { get; set; } = DateTime.UtcNow;

        public string PdfFilePath { get; set; }

        public bool IsActive { get; set; }
    }
}