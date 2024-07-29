namespace TiklaYe_CQRS.Commands
{
    public class CompleteOrderCommand
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public bool SaveCard { get; set; }
        public string DeliveryAddress { get; set; }
        public int UserId { get; set; }
    }
}