using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class AddToCartCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public AddToCartCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(AddToCartCommand command)
        {
            var cartItem = new CartItem
            {
                ProductId = command.ProductId,
                Name = command.Name,
                ImageUrl = command.ImageUrl,
                Price = command.Price,
                Quantity = command.Quantity
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}