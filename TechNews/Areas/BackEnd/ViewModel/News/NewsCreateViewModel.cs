namespace TechNews.Areas.BackEnd.ViewModels.News
{
    public class NewsCreateViewModel
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Content { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? PublishDate { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
