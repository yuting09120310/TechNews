using System;
using System.Collections.Generic;

namespace TechNews.Areas.BackEnd.Models
{
    public partial class MenuSub
    {
        public MenuSub()
        {
            AdminRoles = new HashSet<AdminRole>();
        }

        public int MenuId { get; set; }
        public int MenuGroupId { get; set; }
        public int? SortOrder { get; set; }
        public string Name { get; set; } = null!;
        public string? Permission { get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public bool? IsActive { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual MenuGroup MenuGroup { get; set; } = null!;
        public virtual ICollection<AdminRole> AdminRoles { get; set; }
    }
}
