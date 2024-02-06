using WebApplication1.Users;

namespace WebApplication1.News
{
    public class NewsEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get;  set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int AuthorId { get; set; }
        public UserEntity Author { get; set; }
    }
}
