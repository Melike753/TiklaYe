namespace TiklaYe_CQRS.Queries
{
    public class GetSalesReportQuery
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}