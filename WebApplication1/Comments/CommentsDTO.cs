using System.ComponentModel.DataAnnotations;
using WebApplication1.News;
using WebApplication1.Users;

namespace WebApplication1.Comments
{
    public class CommentsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        public UserDTO Author { get; set; }
        public int NewsId { get; set; }
        public NewsDTO News { get; set; }
    }
    public class CommentCreateDTO
    {
 
        [Required]
        [MinLength(3)]
        public string Content { get; set; }

        [Required]
        public int NewsId { get; set; }
    }
}
