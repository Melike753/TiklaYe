namespace TiklaYe_CQRS.Commands
{
    public class UpdateUserProfileResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
