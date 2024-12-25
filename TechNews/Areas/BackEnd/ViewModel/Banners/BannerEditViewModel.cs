namespace TechNews.Areas.BackEnd.ViewModel.Banners
{
    public class BannerEditViewModel
    {
        public int BannerId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? CurrentImagePath { get; set; }
        public string? ImageAlt { get; set; }
        public bool IsActive { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
