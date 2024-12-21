using System;
using System.Collections.Generic;

namespace TechNews.Admin.Models
{
    public partial class AdminRole
    {
        public int RoleId { get; set; }
        public int GroupId { get; set; }
        public int MenuId { get; set; }
        public string Permission { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }

        public virtual AdminGroup Group { get; set; } = null!;
        public virtual MenuSub Menu { get; set; } = null!;
    }
}
