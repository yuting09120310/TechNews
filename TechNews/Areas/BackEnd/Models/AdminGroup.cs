using System;
using System.Collections.Generic;

namespace TechNews.Areas.BackEnd.Models
{
    public partial class AdminGroup
    {
        public AdminGroup()
        {
            AdminRoles = new HashSet<AdminRole>();
            Admins = new HashSet<Admin>();
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; } = null!;
        public string? GroupInfo { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<AdminRole> AdminRoles { get; set; }
        public virtual ICollection<Admin> Admins { get; set; }
    }
}
