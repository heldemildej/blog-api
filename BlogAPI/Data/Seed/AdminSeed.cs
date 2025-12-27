using BlogAPI.Data;
using BlogAPI.Models;
using BCrypt.Net;
using BlogApi.Models;

namespace BlogAPI.Data.Seed
{
    public static class AdminSeed
    {
        public static void CriarAdmin(BlogContext context)
        {
            // Verifica se já existe algum admin
            if (context.Usuarios.Any(u => u.Role == "Admin"))
                return;

            // Cria o usuário admin
            var admin = new Usuario
            {
                Nome = "Admin",
                Email = "admin@blog.com",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin"
            };

            context.Usuarios.Add(admin);
            context.SaveChanges();
        }
    }
}
