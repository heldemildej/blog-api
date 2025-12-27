using BlogApi.DTOs;
using BlogApi.Models;
using BlogAPI.Data;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly BlogContext _context;
        private readonly JwtService _jwtService;

        public AuthController(BlogContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // POST /auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistrarUsuarioDto dto)
        {
            if (_context.Usuarios.Any(u => u.Email == dto.Email))
                return BadRequest("Email já cadastrado.");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Role = "User"
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return Ok("Usuário criado com sucesso.");
        }

        // POST /auth/login
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == dto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
                return Unauthorized("Credenciais inválidas");

            var token = _jwtService.GerarToken(usuario);

            return Ok(new { token });
        }

        // GET /auth/me
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                Nome = User.Identity?.Name,
                Email = User.FindFirst(ClaimTypes.Email)?.Value,
                Role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }

        // POST /auth/refresh
        [Authorize]
        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
                return Unauthorized();

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (usuario == null)
                return Unauthorized();

            var novoToken = _jwtService.GerarToken(usuario);
            return Ok(new { token = novoToken });
        }
    }
}
