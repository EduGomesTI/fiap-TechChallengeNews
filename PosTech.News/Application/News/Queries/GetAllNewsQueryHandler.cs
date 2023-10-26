using MediatR;
using Microsoft.Extensions.Logging;
using News.Domain.Repositories;

namespace News.Application.News.Queries
{
    internal class GetAllNewsQueryHandler : IRequestHandler<GetAllNewsQuery, IEnumerable<GetAllNewsResponse>>
    {
        private readonly ILogger<GetAllNewsQueryHandler> _logger;
        private readonly INewsRepository _newsRepository;

        public GetAllNewsQueryHandler(ILogger<GetAllNewsQueryHandler> logger, INewsRepository newsRepository)
        {
            _logger = logger;
            _newsRepository = newsRepository;
        }

        public async Task<IEnumerable<GetAllNewsResponse>> Handle(GetAllNewsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retorna todas as notícias.");
            var news = await _newsRepository.GetAllAsync();

            _logger.LogInformation("Converte objeto news para objeto response.");
            IEnumerable<GetAllNewsResponse> result = ConvertToResponse(news);

            return result;
        }

        private static IEnumerable<GetAllNewsResponse> ConvertToResponse(IEnumerable<Domain.Entities.Noticia> news)
        {
            List<GetAllNewsResponse> result = new();

            if (news is null)
            {
                GetAllNewsResponse errorMessage = new();

                errorMessage.AddMessage("Não existem Notícias");

                result.Add(errorMessage);

                return result;
            }

            foreach (var item in news)
            {
                result.Add(new GetAllNewsResponse(item.Id, item.Titulo, item.Descricao, item.DataPublicacao, item.Autor));
            }

            return result;
        }
    }
}