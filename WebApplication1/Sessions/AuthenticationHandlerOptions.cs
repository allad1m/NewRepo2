using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WebApplication1.Sessions
{
    public class AuthenticationHandlerOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "BearerAuthScheme";
        public const string DefaultHeader = "Authorization";
        public string TokenHeaderName { get; set; } = DefaultHeader;
    }
    public class AuthenticationHandler : AuthenticationHandler<AuthenticationHandlerOptions>
    {
        private readonly DatabaseContext _database;
        public AuthenticationHandler(
            IOptionsMonitor<AuthenticationHandlerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            DatabaseContext database
        ) : base(options, logger, encoder, clock)
        {
            _database = database;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(Options.TokenHeaderName))
            {
                return AuthenticateResult.Fail($"Missing header: {Options.TokenHeaderName}");
            }
            string token = Request.Headers[Options.TokenHeaderName]!.First()!.Split(" ").Last();

             var sessionToken = await _database.SessionTokens
                 .Include(t => t.Session)
                 .ThenInclude(t => t.User)
                 .Where(t => t.Token == Guid.Parse(token) && t.Type == SessionTokenType.Access)
                 .FirstOrDefaultAsync();


             if (sessionToken == null)
             {
                 return AuthenticateResult.Fail("Session not found");
             }

             var user = sessionToken.Session.User;

             var claims = new List<Claim>
             {
                 new Claim(ClaimTypes.Name, user.Id.ToString()),
             };

             var identity = new ClaimsIdentity(claims, this.Scheme.Name);
             var principal = new ClaimsPrincipal(identity);

             return AuthenticateResult.Success(new(principal, this.Scheme.Name));
        }
    }

}
