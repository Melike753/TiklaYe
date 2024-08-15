namespace TiklaYe_CQRS.Models
{
    public class RestaurantRevenueViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalAmount { get; set; }
        public List<BusinessOwnerSalesReportViewModel> Sales { get; set; } = new List<BusinessOwnerSalesReportViewModel>();
    }
}