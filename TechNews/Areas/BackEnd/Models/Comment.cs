using System;
using System.Collections.Generic;

namespace TechNews.Areas.BackEnd.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int NewsId { get; set; }
        public string UserName { get; set; } = null!;
        public string? Email { get; set; }
        public string Message { get; set; } = null!;
        public DateTime CreateDate { get; set; }

        public virtual News News { get; set; } = null!;
    }
}
