namespace News.Domain.Entities
{
    public sealed record Noticia
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public string Autor { get; set; } = string.Empty;

        public Noticia()
        {
        }

        public Noticia(string titulo, string descricao, DateTime dataPublicacao, string autor)
        {
            Titulo = titulo;
            Descricao = descricao;
            DataPublicacao = ValidateDate(dataPublicacao);
            Autor = autor;
        }

        public Noticia(int id, string titulo, string descricao, DateTime dataPublicacao, string autor) : this(titulo, descricao, dataPublicacao, autor)
        {
            Id = id;
        }

        private static DateTime ValidateDate(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return DateTime.Now;
            }

            return dateTime;
        }
    }
}