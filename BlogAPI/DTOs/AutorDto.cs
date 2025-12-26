using System.ComponentModel.DataAnnotations;

public class AutorDto
{
    public int Id { get; set; }
    [Required]
    [MinLength(3)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}