using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; } = null!;
    }
}
