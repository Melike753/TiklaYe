using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Commands
{
    public class ProductCreateCommand
    {
        public Product Product { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}