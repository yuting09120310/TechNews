namespace TechNews.Areas.BackEnd.ViewModel.Admins
{
    public class AdminsIndexViewModel
    {
        public int AdminId { get; set; }
        public string AdminAccount { get; set; } = string.Empty;
        public string AdminName { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

}
