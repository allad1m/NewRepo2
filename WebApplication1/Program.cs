using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.ComponentModel;
using System.Reflection;
using WebApplication1;
using WebApplication1.Sessions;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite("DataSource = AppData/database.sqlite"));

        // Add services to the container.

        builder.Services.AddAutoMapper(typeof(AppMappingProfile));
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services
            .AddAuthentication(AuthenticationHandlerOptions.DefaultScheme)
            .AddScheme<AuthenticationHandlerOptions, AuthenticationHandler>(AuthenticationHandlerOptions.DefaultScheme, null);

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Веб-сервер",
                Description = "Тут публикуют новости",
                TermsOfService = new Uri("http://nke.ru"),
                Contact = new OpenApiContact
                {
                    Name = "Контакты",
                    Url = new Uri("http://nke.ru")
                },
                License = new OpenApiLicense
                {
                    Name = "Лицензия",
                    Url = new Uri("http://nke.ru")
                }
            });

            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Укажите токен, полученный через POST /auth",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "UUID",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();

        await app.RunAsync();
    }
}


