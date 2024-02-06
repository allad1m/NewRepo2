using System.ComponentModel.DataAnnotations;
using WebApplication1.Users;

namespace WebApplication1.Sessions
{
    public class AccountDTO
    {
        public UserDTO User { get; set; }
    }

    public class LoginDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }

    public class TokenDTO
    {
        public string Token { get; set; }
        public SessionTokenType Type { get; set; }
    }
}
