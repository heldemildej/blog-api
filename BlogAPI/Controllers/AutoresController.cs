using BlogAPI.Data;
using BlogAPI.DTOs;
using BlogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("autores")]
    public class AutoresController : ControllerBase
    {
        private readonly BlogContext _context;

        public AutoresController(BlogContext context)
        {
            _context = context;
        }

        // GET /autores
        [HttpGet]
        public IActionResult ListarAutores()
        {
            var autores = _context.Autores
                .Select(a => new AutorDto
                {
                    Nome = a.Nome,
                    Email = a.Email
                }).ToList();

            return Ok(autores);
        }

        // GET /autores/{id}
        [HttpGet("{id}")]
        public IActionResult ObterAutor(int id)
        {
            var autor = _context.Autores
                .Where(a => a.Id == id)
                .Select(a => new AutorDto
                {
                    Nome = a.Nome,
                    Email = a.Email
                }).FirstOrDefault();

            if (autor == null)
                return NotFound("Autor não encontrado.");

            return Ok(autor);
        }

        // POST /autores
        [HttpPost]
        public IActionResult CriarAutor([FromBody] AutorDto dto)
        {
            if (dto == null)
                return BadRequest("Dados inválidos.");

            var autor = new Autor
            {
                Nome = dto.Nome,
                Email = dto.Email
            };

            _context.Autores.Add(autor);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterAutor), new { id = autor.Id }, new AutorDto
            {
                Nome = autor.Nome,
                Email = autor.Email
            });
        }

        // PUT /autores/{id}
        [HttpPut("{id}")]
        public IActionResult AtualizarAutor(int id, [FromBody] AutorDto dto)
        {
            var autor = _context.Autores.Find(id);
            if (autor == null)
                return NotFound("Autor não encontrado.");

            autor.Nome = dto.Nome;
            autor.Email = dto.Email;
            _context.SaveChanges();

            return Ok(new AutorDto
            {
                Nome = autor.Nome,
                Email = autor.Email
            });
        }

        // DELETE /autores/{id}
        [HttpDelete("{id}")]
        public IActionResult RemoverAutor(int id)
        {
            var autor = _context.Autores.Find(id);
            if (autor == null)
                return NotFound("Autor não encontrado.");

            _context.Autores.Remove(autor);
            _context.SaveChanges();

            return NoContent();
        }

        // GET /autores/{id}/posts
        [HttpGet("{id}/posts")]
        public IActionResult PostsDoAutor(int id)
        {
            var autorExiste = _context.Autores.Any(a => a.Id == id);
            if (!autorExiste)
                return NotFound("Autor não encontrado.");

            var posts = _context.Posts
                .Where(p => p.AutorId == id)
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
                        Nome = p.Autor.Nome,
                        Email = p.Autor.Email
                    }
                }).ToList();

            return Ok(posts);
        }
    }
}
