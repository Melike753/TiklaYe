namespace TiklaYe_CQRS.Commands
{
    public class RemoveFromCartCommand
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}