using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{
    public class Autor
    {
            [Key]
            public int Id { get; set; }

            [Required]
            public string Nome { get; set; } = string.Empty;

            [Required]
            public string Email { get; set; } = string.Empty;

        // Um autor pode ter vários posts
        public List<Post> Posts { get; set; } = new List<Post>();
     
    }
}
