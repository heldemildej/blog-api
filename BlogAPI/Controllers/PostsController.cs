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

        // GET /posts
        [HttpGet]
        public async Task<IActionResult> Listar(
            [FromQuery] string? search,
            [FromQuery] string? category,
            [FromQuery] int? author)
        {
            var posts = _context.Posts.Include(p => p.Autor).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                posts = posts.Where(p => p.Titulo.Contains(search));

            if (!string.IsNullOrEmpty(category))
                posts = posts.Where(p => p.Categoria.ToLower() == category.ToLower());

            if (author.HasValue)
                posts = posts.Where(p => p.AutorId == author.Value);

            var result = posts.Select(p => new PostResponseDto
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Conteudo = p.Conteudo,
                Categoria = p.Categoria,
                CriadoEm = p.CriadoEm,
                Views = p.Views,
                Autor = new AutorDto
                {
                    Id = p.Autor.Id,
                    Nome = p.Autor.Nome,
                    Email = p.Autor.Email
                }
            });

            return Ok(await result.ToListAsync());
        }

        // GET /posts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Obter(int id)
        {
            var post = await _context.Posts
                .Include(p => p.Autor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound("Post não encontrado.");

            var dto = new PostResponseDto
            {
                Id = post.Id,
                Titulo = post.Titulo,
                Conteudo = post.Conteudo,
                Categoria = post.Categoria,
                CriadoEm = post.CriadoEm,
                Views = post.Views,
                Autor = new AutorDto
                {
                    Id = post.Autor.Id,
                    Nome = post.Autor.Nome,
                    Email = post.Autor.Email
                }
            };

            return Ok(dto);
        }

        // POST /posts
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] PostDto dto)
        {
            var autor = await _context.Autores.FindAsync(dto.AutorId);
            if (autor == null) return BadRequest("Autor não encontrado.");

            var post = new Post
            {
                Titulo = dto.Titulo,
                Conteudo = dto.Conteudo,
                Categoria = dto.Categoria,
                AutorId = dto.AutorId
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Retornando com DTO para evitar ciclo
            var response = new PostResponseDto
            {
                Id = post.Id,
                Titulo = post.Titulo,
                Conteudo = post.Conteudo,
                Categoria = post.Categoria,
                CriadoEm = post.CriadoEm,
                Views = post.Views,
                Autor = new AutorDto
                {
                    Id = autor.Id,
                    Nome = autor.Nome,
                    Email = autor.Email
                }
            };

            return Ok(response);
        }

        // PUT /posts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] PostDto dto)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound("Post não encontrado.");

            post.Titulo = dto.Titulo;
            post.Conteudo = dto.Conteudo;
            post.Categoria = dto.Categoria;
            post.AutorId = dto.AutorId;

            await _context.SaveChangesAsync();
            return Ok(post);
        }

        // DELETE /posts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound("Post não encontrado.");

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET /posts/recent
        [HttpGet("recent")]
        public async Task<IActionResult> Recentes()
        {
            var posts = await _context.Posts
                .Include(p => p.Autor)
                .OrderByDescending(p => p.CriadoEm)
                .Take(5)
                .Select(p => new PostResponseDto
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Conteudo = p.Conteudo,
                    Categoria = p.Categoria,
                    CriadoEm = p.CriadoEm,
                    Views = p.Views,
                    Autor = new AutorDto
                    {
                        Id = p.Autor.Id,
                        Nome = p.Autor.Nome,
                        Email = p.Autor.Email
                    }
                })
                .ToListAsync();

            return Ok(posts);
        }

        // GET /posts/popular
        [HttpGet("popular")]
        public async Task<IActionResult> Populares()
        {
            var posts = await _context.Posts
                .Include(p => p.Autor)
                .OrderByDescending(p => p.Views)
                .Take(5)
                .Select(p => new PostResponseDto
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Conteudo = p.Conteudo,
                    Categoria = p.Categoria,
                    CriadoEm = p.CriadoEm,
                    Views = p.Views,
                    Autor = new AutorDto
                    {
                        Id = p.Autor.Id,
                        Nome = p.Autor.Nome,
                        Email = p.Autor.Email
                    }
                })
                .ToListAsync();

            return Ok(posts);
        }
    }
}
