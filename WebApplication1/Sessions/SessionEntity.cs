using WebApplication1.Users;

namespace WebApplication1.Sessions
{
    public enum SessionTokenType
    {
        Access,
        Refresh
    }
    public class SessionTokenEntity
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public SessionEntity Session { get; set; }
        public Guid Token { get; set; }
        public SessionTokenType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class SessionEntity
    {
        public int Id { get; set; }
        public string UserAgent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public List<SessionTokenEntity> Tokens { get; set; }
    }
}
