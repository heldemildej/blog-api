using BlogAPI.Data;
using BlogAPI.DTOs;
using BlogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostsController : ControllerBase
    {
        private readonly BlogContext _context;

        public PostsController(BlogContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] string? search)
        {
            var posts = _context.Posts.Include(p => p.Autor).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                posts = posts.Where(p => p.Titulo.Contains(search));

            return Ok(await posts.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Criar(PostDto dto)
        {
            var autor = await _context.Autores.FindAsync(dto.AutorId);
            if (autor == null)
                return BadRequest("Autor não encontrado.");

            var post = new Post
            {
                Titulo = dto.Titulo,
                Conteudo = dto.Conteudo,
                AutorId = dto.AutorId,
                Categoria = dto.Categoria
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return Ok(post);
        }
    }
}
