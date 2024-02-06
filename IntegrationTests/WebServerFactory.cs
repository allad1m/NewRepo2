using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApplication1;

namespace IntegrationTests
{
    public class WebServerFactory<TProgram>:
        WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddAuthentication(defaultScheme: "TestScheme")
        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });

                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));
                services.Remove(dbContextDescriptor);
                services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("TextData"));
                //создаем сервисы и получаем доступ к БД
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                using var appContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                Utilites.ReinitializeDbForTests(appContext);
            });
        }
        protected override void ConfigureClient(HttpClient client)
        {
            //при использовании тестовой схемы авторизации никаких токенов не используется - мы просто укажем серверу, что пришли чисто потестировать
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "TestScheme");
        }

    }
}
