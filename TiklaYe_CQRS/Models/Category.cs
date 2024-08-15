using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TiklaYe_CQRS.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [NotMapped] // Veritabanına yansıtılmayacak
        public IFormFile? ImageUrlFile { get; set; } // Dosya yükleme için kullanılacak.
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}