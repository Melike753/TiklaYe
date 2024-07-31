using System.ComponentModel.DataAnnotations;

namespace TiklaYe_CQRS.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}