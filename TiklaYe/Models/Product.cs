using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace TiklaYe.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Fiyat gereklidir.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stok miktarı gereklidir.")]
        public int Quantity { get; set; }

        [NotMapped]
        public IFormFile? ImageUrlFile { get; set; } // Dosya yükleme için kullanılacak

        public string? ImageUrl { get; set; } // Veritabanında tutulan dosya yolu

        [Required(ErrorMessage = "Kategori gereklidir.")]
        public int CategoryId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Category Category { get; set; }
    }
}
