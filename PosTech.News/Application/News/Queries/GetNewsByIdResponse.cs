using News.Application.Abstractions;

namespace News.Application.News.Queries
{
    public sealed record GetNewsByIdResponse : BaseResponse
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public DateTime DataPublicacao { get; set; }

        public string Autor { get; set; } = string.Empty;

        public GetNewsByIdResponse()
        {
        }

        public GetNewsByIdResponse(int id, string titulo, string descricao, DateTime dataPublicacao, string autor)
        {
            Id = id;
            Titulo = titulo;
            Descricao = descricao;
            DataPublicacao = dataPublicacao;
            Autor = autor;
        }
    }
}