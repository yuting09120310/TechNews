using System;
using System.Collections.Generic;

namespace TechNews.Admin.Models
{
    public partial class Banner
    {
        public int BannerId { get; set; }
        public string LanguageCode { get; set; } = null!;
        public int? CategoryId { get; set; }
        public int? SortOrder { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageAlt { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
