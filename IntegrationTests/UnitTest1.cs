using System.Net;
using System.Net.Http.Json;
using WebApplication1.Users;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class UnitTest1 :
        IClassFixture<WebServerFactory<Program>>
    {
        private readonly WebServerFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;
        public UnitTest1(WebServerFactory<Program> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _client = factory.CreateDefaultClient();
            _output = output;
        }
        [Fact]
        public async Task GetNews_Success()
        {
            var response = await _client.GetAsync("/news");
            _output.WriteLine(await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
        }
        [Theory]
        [InlineData("/news")]
        [InlineData("/comments/1")]
        [InlineData("/auth")]
        public async Task GetAll_Success(string path)
        {
            var response = await _client.GetAsync(path);
            _output.WriteLine(await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task GetUnexistingComments_NotFound()
        {
            var response = await _client.GetAsync("/comments?newsId=9999");

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAccountWithoutAuth_Forbid()
        {
            //создаем клиент без авторизации
            var client = _factory.CreateDefaultClient();
            client.DefaultRequestHeaders.Authorization = null;
            //проверяем метод
            var response = await client.GetAsync("/auth");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
        [Fact]
        public async Task ChangeEmail_Success()
        {
            var responce = await _client.PostAsJsonAsync("/auth/email", new EmailChangeDTO() { 
                Email = "ivanov@example.ru"
            });
            responce.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task ChangePassword_Success()
        {
            var responce = await _client.PostAsJsonAsync("/auth/password", new PasswordChangeDTO()
            {
                Password = "1qaz!QAZ"
            });
            responce.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task ChangeFio_Success()
        {
            var responce = await _client.PostAsJsonAsync("/auth/fio", new ChangeFioDTO()
            {
                Name = "string",
                Family = "string",
                Patronymic = "string",
            });
            responce.EnsureSuccessStatusCode();
        }
    }
}