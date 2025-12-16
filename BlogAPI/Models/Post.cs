using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{
    public class Post
    {
            [Key] // chave primária
            public int Id { get; set; }

            [Required]
            public string Titulo { get; set; } = string.Empty;

            [Required]
            public string Conteudo { get; set; } = string.Empty;

            [Required]
            [ForeignKey("Autor")]
            public int AutorId { get; set; }
            public Autor Autor { get; set; } = null!;

            public string Categoria { get; set; } = string.Empty;

            public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

            public int Views { get; set; } = 0;
     }
}