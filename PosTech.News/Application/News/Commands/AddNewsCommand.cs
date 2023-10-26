using MediatR;

namespace News.Application.News.Commands
{
    public sealed record AddNewsCommand : IRequest<AddNewsResponse>
    {
        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public DateTime DataPublicacao { get; set; }

        public string Autor { get; set; } = string.Empty;

        public AddNewsCommand()
        {
        }

        public AddNewsCommand(string titulo, string descricao, DateTime dataPublicacao, string autor)
        {
            Titulo = titulo;
            Descricao = descricao;
            DataPublicacao = dataPublicacao;
            Autor = autor;
        }
    }
}