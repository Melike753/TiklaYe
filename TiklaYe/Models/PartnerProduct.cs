using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace TiklaYe.Models
{
    public class PartnerProduct
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Ürün adı gereklidir.")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Fiyat gereklidir.")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped] 
        public IFormFile? ImageFile { get; set; } // Veritabanı sütunu olarak saklanmaz.

        public bool IsActive { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? BusinessOwnerId { get; set; }
        public BusinessOwner? BusinessOwner { get; set; }
    }
}