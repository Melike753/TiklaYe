namespace TiklaYe_CQRS.Commands
{
    public class UpdateCategoryCommand
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public IFormFile ImageUrlFile { get; set; }
    }
}