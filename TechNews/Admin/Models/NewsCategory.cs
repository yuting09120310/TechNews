using System;
using System.Collections.Generic;

namespace TechNews.Admin.Models
{
    public partial class NewsCategory
    {
        public NewsCategory()
        {
            InverseParent = new HashSet<NewsCategory>();
            News = new HashSet<News>();
        }

        public int CategoryId { get; set; }
        public int? StoreId { get; set; }
        public int? SortOrder { get; set; }
        public string CategoryCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int? Level { get; set; }
        public int? ParentId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual NewsCategory? Parent { get; set; }
        public virtual ICollection<NewsCategory> InverseParent { get; set; }
        public virtual ICollection<News> News { get; set; }
    }
}
