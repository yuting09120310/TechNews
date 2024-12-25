namespace TechNews.Areas.BackEnd.ViewModel.Banners
{
    public class BannerCreateViewModel
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImageAlt { get; set; }
        public bool IsActive { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
