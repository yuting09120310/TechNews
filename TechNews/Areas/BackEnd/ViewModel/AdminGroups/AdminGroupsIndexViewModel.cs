namespace TechNews.Areas.BackEnd.ViewModel.AdminGroups
{
    public class AdminGroupsIndexViewModel
    {
        public int GroupId { get; set; } // 群組 ID
        public string GroupName { get; set; } = string.Empty; // 群組名稱
        public string GroupInfo { get; set; } = string.Empty; // 群組資訊
        public bool IsActive { get; set; } // 是否啟用
        public DateTime ModifiedDate { get; set; } // 修改日期
    }

}
