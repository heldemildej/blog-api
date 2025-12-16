namespace BlogAPI.DTOs
{
    public class PostDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        public int AutorId { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }
}
