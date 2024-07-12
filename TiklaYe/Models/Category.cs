using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiklaYe.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string? Name { get; set; }
        [NotMapped] // Veritabanına yansıtılmayacak
        public IFormFile? ImageUrlFile { get; set; } // Dosya yükleme için kullanılacak
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
