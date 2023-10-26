namespace News.Domain.Entities
{
    public sealed record Noticia
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public string Autor { get; set; } = string.Empty;
    }
}