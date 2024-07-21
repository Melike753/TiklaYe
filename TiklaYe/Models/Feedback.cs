using System;
using System.ComponentModel.DataAnnotations;

namespace TiklaYe.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
