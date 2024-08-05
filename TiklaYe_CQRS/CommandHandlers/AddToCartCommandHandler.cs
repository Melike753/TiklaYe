using Microsoft.EntityFrameworkCore;
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
            // Kullanıcının sepetini bul
            var userCart = await _context.CartItems
                .Where(ci => ci.UserId == command.UserId)
                .ToListAsync();

            var product = await _context.PartnerProducts
                .FirstOrDefaultAsync(p => p.ProductId == command.ProductId);

            if (product == null)
            {
                throw new Exception("Ürün bulunamadı.");
            }

            // Mevcut sepetin restoran ID'sini kontrol et
            var currentBusinessOwnerId = userCart.FirstOrDefault()?.BusinessOwnerId;

            if (currentBusinessOwnerId.HasValue && currentBusinessOwnerId.Value != product.BusinessOwnerId)
            {
                throw new Exception("Farklı restoranlardan ürün ekleyemezsiniz.");
            }

            var userCartItem = userCart
                .FirstOrDefault(ci => ci.ProductId == command.ProductId);

            if (userCartItem != null)
            {
                userCartItem.Quantity += command.Quantity;
            }
            else
            {
                var newCartItem = new CartItem
                {
                    UserId = command.UserId,
                    ProductId = command.ProductId,
                    Name = command.Name,
                    ImageUrl = command.ImageUrl,
                    Price = command.Price,
                    Quantity = command.Quantity,
                    BusinessOwnerId = (int)product.BusinessOwnerId // Restoran ID'sini ekleyin
                };
                await _context.CartItems.AddAsync(newCartItem);
            }

            await _context.SaveChangesAsync();
        }
    }
}