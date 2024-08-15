namespace TiklaYe_CQRS.Models
{
    public class PurchaseGroupViewModel
    {
        public string OrderNumber { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Status { get; set; }
        public List<Purchase> Items { get; set; }
    }
}