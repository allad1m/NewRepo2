using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Users
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Family { get; set; } = "";
        public string Name { get; set; } = "";
        public string Patronymic { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class UserCreateDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Family { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Пароль должен соответствовать требованиям, включать в себя заглавные и прописные английские буквы, и специальный символ, а также быть минимум 8 символов.")]
        public string? Password { get; set; }
        public string? Patronymic { get; set; }
    }

    public class EmailChangeDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ChangeFioDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Family { get; set; }
        public string? Patronymic { get; set; }
    }

    public class PasswordChangeDTO
    {
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Пароль должен соответствовать требованиям, включать в себя заглавные и прописные английские буквы, и специальный символ, а также быть минимум 8 символов.")]
        public string Password { get; set; }
    }
}
