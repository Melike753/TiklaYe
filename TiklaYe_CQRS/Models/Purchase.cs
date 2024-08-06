namespace TiklaYe_CQRS.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int UserId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string OrderNumber { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } 
        public User User { get; set; }
        public int BusinessOwnerId { get; set; }
        public BusinessOwner BusinessOwner { get; set; }
    }
}