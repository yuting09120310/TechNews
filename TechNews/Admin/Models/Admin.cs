using System;
using System.Collections.Generic;

namespace TechNews.Admin.Models
{
    public partial class Admin
    {
        public int AdminId { get; set; }
        public int GroupId { get; set; }
        public string AdminAccount { get; set; } = null!;
        public string AdminPassword { get; set; } = null!;
        public string AdminName { get; set; } = null!;
        public bool? IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual AdminGroup Group { get; set; } = null!;
    }
}
