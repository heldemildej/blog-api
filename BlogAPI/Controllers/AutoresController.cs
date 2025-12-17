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
            var autores = _context.Autores.ToList();
            return Ok(autores);
        }

        // GET /autores/{id}
        [HttpGet("{id}")]
        public IActionResult ObterAutor(int id)
        {
            var autor = _context.Autores.Find(id);
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

            return CreatedAtAction(nameof(ObterAutor), new { id = autor.Id }, autor);
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

            return Ok(autor);
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
                .Include(p => p.Autor)
                .Where(p => p.AutorId == id)
                .ToList();

            return Ok(posts);
        }
    }
}
