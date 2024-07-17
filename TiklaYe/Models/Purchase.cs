namespace TiklaYe.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int UserId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }

        public User User { get; set; }
    }
}
