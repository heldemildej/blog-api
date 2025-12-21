using System.ComponentModel.DataAnnotations;

namespace BlogApi.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(3)]
        public string Senha { get; set; } = string.Empty;
    }
}
