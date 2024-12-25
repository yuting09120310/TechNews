namespace TechNews.Areas.BackEnd.ViewModel.Admins
{
    public class AdminsEditViewModel
    {
        public int AdminId { get; set; }
        public string AdminAccount { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;
        public string AdminName { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public bool IsActive { get; set; }
    }
}
