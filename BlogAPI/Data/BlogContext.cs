using BlogApi.Models;
using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Data
{
    public class BlogContext : DbContext
    {
        private readonly IConfiguration _config;

        public BlogContext(DbContextOptions<BlogContext> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Autor> Autores { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;

    }
}
