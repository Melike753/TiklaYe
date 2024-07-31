using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Commands
{
    public class ProductUpdateCommand
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}