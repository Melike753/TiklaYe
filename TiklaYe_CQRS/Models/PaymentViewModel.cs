namespace TiklaYe_CQRS.Models
{
    public class PaymentViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}