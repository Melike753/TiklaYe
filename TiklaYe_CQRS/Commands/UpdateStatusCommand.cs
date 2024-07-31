namespace TiklaYe_CQRS.Commands
{
    public class UpdateStatusCommand
    {
        public string OrderNumber { get; set; }
        public string Status { get; set; }
    }
}