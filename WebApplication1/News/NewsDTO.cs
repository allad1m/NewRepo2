using WebApplication1.Users;

namespace WebApplication1.News
{
    public class NewsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        public UserDTO Author { get; set; }
    }
}
