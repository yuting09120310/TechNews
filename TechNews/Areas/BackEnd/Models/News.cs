using System;
using System.Collections.Generic;

namespace TechNews.Areas.BackEnd.Models
{
    public partial class News
    {
        public News()
        {
            Comments = new HashSet<Comment>();
        }

        public int NewsId { get; set; }
        public int? StoreId { get; set; }
        public string LanguageCode { get; set; } = null!;
        public int CategoryId { get; set; }
        public int? SortOrder { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageAlt { get; set; }
        public bool? IsActive { get; set; }
        public int? ViewCount { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? Tags { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual NewsCategory Category { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
