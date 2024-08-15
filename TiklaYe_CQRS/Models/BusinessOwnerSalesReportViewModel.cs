namespace TiklaYe_CQRS.Models
{
    public class BusinessOwnerSalesReportViewModel
    {
        public decimal TotalAmount { get; set; }
        public List<PurchaseItemViewModel> PurchasedItems { get; set; } = new List<PurchaseItemViewModel>();
    }
}