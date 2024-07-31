using System.ComponentModel.DataAnnotations;

namespace TiklaYe_CQRS.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
    }
}