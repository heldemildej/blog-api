using BlogApi.DTOs;
using BlogApi.Models;
using BlogAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly BlogContext _context;
        private readonly IConfiguration _config;

        public AuthController(BlogContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
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
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == dto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
                return Unauthorized("Credenciais inválidas.");

            var token = GerarToken(usuario);
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

        // POST /auth/refresh (versão simples)
        [Authorize]
        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            var nome = User.Identity?.Name;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (email == null)
                return Unauthorized();

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (usuario == null)
                return Unauthorized();

            var novoToken = GerarToken(usuario);
            return Ok(new { token = novoToken });
        }

        //-----> JWT
        private string GerarToken(Usuario usuario)
        {
            var jwt = _config.GetSection("Jwt");
            var chave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["ChaveSecreta"])
            );

            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Emissor"],
                audience: jwt["Audiencia"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(jwt["ExpiracaoEmMinutos"])
                ),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
