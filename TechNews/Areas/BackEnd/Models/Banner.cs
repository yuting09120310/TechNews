using System;
using System.Collections.Generic;

namespace TechNews.Areas.BackEnd.Models
{
    public partial class Banner
    {
        public int BannerId { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageAlt { get; set; }
        public bool? IsActive { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
