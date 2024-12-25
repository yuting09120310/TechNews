using System.ComponentModel.DataAnnotations;
using TechNews.Areas.BackEnd.Models;

namespace TechNews.Areas.BackEnd.ViewModel.AdminGroups
{
    public class AdminGroupsEditViewModel
    {
        [Display(Name = "群組編號")]
        public long GroupId { get; set; }

        [Display(Name = "群組名稱")]
        public string? GroupName { get; set; }

        [Display(Name = "說明")]
        public string? GroupInfo { get; set; }

        [Display(Name = "狀態")]
        public bool? IsActive { get; set; }

        public List<AdminRole> AdminRoleModels { get; set; }

        public List<MenuGroup> MenuGroupModels { get; set; }

        public List<MenuSub> MenuSubModels { get; set; }
    }
}
