using System.ComponentModel.DataAnnotations;

namespace BlogApi.DTOs
{
    public class RegistrarUsuarioDto
    {
        [Required]
        [MinLength(3)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Senha { get; set; } = string.Empty;
    }
}
