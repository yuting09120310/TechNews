namespace TechNews.Areas.BackEnd.ViewModels.News
{
    public class NewsIndexViewModel
    {
        public int NewsId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? ViewCount { get; set; }
    }
}
