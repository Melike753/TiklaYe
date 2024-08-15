using System.ComponentModel.DataAnnotations;

namespace TiklaYe_CQRS.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(50)]
        public string Mobile { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string PostCode { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(256)]
        public string Password { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}