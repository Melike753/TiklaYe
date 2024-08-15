namespace TiklaYe_CQRS.Commands
{
    public class RegisterUserCommand
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}