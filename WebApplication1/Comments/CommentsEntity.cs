using WebApplication1.News;
using WebApplication1.Users;

namespace WebApplication1.Comments
{
    public class CommentsEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int AuthorId { get; set; }
        public UserEntity Author { get; set; }
        public int NewsId { get; set; }
        public NewsEntity News { get; set; }
    }
}
