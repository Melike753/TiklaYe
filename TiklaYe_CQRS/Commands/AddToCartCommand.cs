namespace TiklaYe_CQRS.Commands
{
    public class AddToCartCommand
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
    }
}