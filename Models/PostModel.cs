using DotkonBlog.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace DotkonBlog.Models
{
    public class PostModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
