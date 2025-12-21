using System.ComponentModel.DataAnnotations;

namespace BlogAPI.DTOs
{
    public class PostDto
    {
        [Required]
        [MinLength(5)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [MinLength(20)]
        public string Conteudo { get; set; } = string.Empty;

        [Required]
        public int AutorId { get; set; }

        [Required]
        public string Categoria { get; set; } = string.Empty;
    }
}
