namespace TiklaYe_CQRS.Commands
{
    public class AddProductCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public int BusinessOwnerId { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }
    }
}