using System;
using System.Collections.Generic;

namespace TechNews.Admin.Models
{
    public partial class MenuGroup
    {
        public MenuGroup()
        {
            MenuSubs = new HashSet<MenuSub>();
        }

        public int MenuGroupId { get; set; }
        public int? MenuSort { get; set; }
        public string Name { get; set; } = null!;
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public bool? IsActive { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<MenuSub> MenuSubs { get; set; }
    }
}
