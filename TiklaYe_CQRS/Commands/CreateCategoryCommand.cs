namespace TiklaYe_CQRS.Commands
{
    public class CreateCategoryCommand
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public IFormFile ImageUrlFile { get; set; }
    }
}