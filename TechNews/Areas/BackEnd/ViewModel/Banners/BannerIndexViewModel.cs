namespace TechNews.Areas.BackEnd.ViewModel.Banners
{
    public class BannerIndexViewModel
    {
        public int BannerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
