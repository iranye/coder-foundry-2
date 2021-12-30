using System;
using System.ComponentModel.DataAnnotations;

namespace IrasBlog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        
        [Required]
        public int BlogPostId { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public string CommentBody { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdateReason { get; set; }

        public ApplicationUser Author { get; set; }

        public BlogPost BlogPost { get; set; }
    }
}
