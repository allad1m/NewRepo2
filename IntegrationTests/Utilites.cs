using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1;
using WebApplication1.Migrations;
using WebApplication1.Sessions;

namespace IntegrationTests
{
    public static class Utilites
    {
        public static void InitializeDbForTests(DatabaseContext db)
        {
            db.Users.Add(new()
            {
                Family = "Test",
                Name = "Test",
                Patronymic = "Test",
                Email = "Test",
                Password = "Test",
            });
            db.Sessions.AddRange(new List<SessionEntity>()
            {
                new SessionEntity()
                {
                    UserAgent = "Test",
                    UserId = 1,
                    Tokens = new List<SessionTokenEntity>()
                    {
                        new SessionTokenEntity()
                        {
                            Token = Guid.NewGuid(),
                            Type = SessionTokenType.Access
                        }
                    }
                }
            });
            db.News.Add(new()
            {
                AuthorId = 1,
                Title = "Test",
                Content = "Test",
            });
            db.Comments.Add(new()
            {
                NewsId = 1,
                Content = "Test",
                AuthorId = 1,
            });
            db.SaveChanges();
        }
        public static void ReinitializeDbForTests(DatabaseContext db)
        {
            //комментарии удалятся автоматически при удалекнии связанных с ними новостей
            db.News.RemoveRange(db.News);
            db.Sessions.RemoveRange(db.Sessions);
            //удаляем только юзеров, созданных для тестов.
            db.Users.RemoveRange(db.Users.Where(u => u.Id > 1)); InitializeDbForTests(db);

        }
    }
}
