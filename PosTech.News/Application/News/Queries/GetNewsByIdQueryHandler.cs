using MediatR;
using Microsoft.Extensions.Logging;
using News.Domain.Entities;
using News.Domain.Repositories;

namespace News.Application.News.Queries
{
    internal class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, GetNewsByIdResponse>
    {
        private readonly ILogger<GetNewsByIdQueryHandler> _logger;
        private readonly INewsRepository _newsRepository;

        public GetNewsByIdQueryHandler(ILogger<GetNewsByIdQueryHandler> logger, INewsRepository newsRepository)
        {
            _logger = logger;
            _newsRepository = newsRepository;
        }

        public async Task<GetNewsByIdResponse> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retorna a notícia do Id enviado.");
            var news = await _newsRepository.GetByIdAsync(request.Id);

            _logger.LogInformation("Converte objeto news para objeto response.");
            GetNewsByIdResponse result = ConvertToResponse(news);

            return result;
        }

        private static GetNewsByIdResponse ConvertToResponse(Noticia news)
        {
            GetNewsByIdResponse result = new();

            if (news is null)
            {
                result.AddMessage("Notícia não encontrada pelo id informado");

                return result;
            }

            result = new(news!.Id, news.Titulo, news.Descricao, news.DataPublicacao, news.Autor);

            return result;
        }
    }
}