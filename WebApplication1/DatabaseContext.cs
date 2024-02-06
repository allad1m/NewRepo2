using Microsoft.EntityFrameworkCore;
using WebApplication1.Comments;
using WebApplication1.News;
using WebApplication1.Sessions;
using WebApplication1.Users;

namespace WebApplication1
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasData(new UserEntity
            {
                Id = 1,
                Family = "Иванов",
                Name = "Иван",
                Patronymic = "Иванович",
                Email = "ivanov@example.com",
                Password = "password",
            });
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();

        public DbSet<NewsEntity> News => Set<NewsEntity>();

        public DbSet<CommentsEntity> Comments => Set<CommentsEntity>();

        public DbSet<SessionEntity> Sessions => Set<SessionEntity>();

        public DbSet<SessionTokenEntity> SessionTokens => Set<SessionTokenEntity>();
    }
}
