namespace BlogAPI.DTOs
{
    public class PostResponseDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public AutorDto Autor { get; set; } = null!;
        public DateTime CriadoEm { get; set; }
        public int Views { get; set; }
    }
}
