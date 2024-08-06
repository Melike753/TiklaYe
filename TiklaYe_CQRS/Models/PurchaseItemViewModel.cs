namespace TiklaYe_CQRS.Models
{
    public class PurchaseItemViewModel
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}