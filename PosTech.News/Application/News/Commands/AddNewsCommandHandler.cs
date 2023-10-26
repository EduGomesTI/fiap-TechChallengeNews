using MediatR;
using Microsoft.Extensions.Logging;
using News.Application.Abstractions;
using News.Domain.Entities;
using News.Domain.Repositories;

namespace News.Application.News.Commands
{
    internal sealed class AddNewsCommandHandler : IRequestHandler<AddNewsCommand, AddNewsResponse>
    {
        private readonly ILogger<AddNewsCommandHandler> _logger;
        private readonly INewsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AddNewsCommandHandler(ILogger<AddNewsCommandHandler> logger, INewsRepository repository, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddNewsResponse> Handle(AddNewsCommand request, CancellationToken cancellationToken)
        {
            //TODO: validar request

            _logger.LogInformation("Adiciona uma nova notícia.");
            Noticia noticia = new()
            {
                Titulo = request.Titulo,
                Descricao = request.Descricao,
                DataPublicacao = request.DataPublicacao,
                Autor = request.Autor
            };

            await _repository.InsertAsync(noticia);

            var returnOfSaveChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);

            AddNewsResponse result = new();

            if (returnOfSaveChanges == 0)
            {
                result.AddMessage($"Ocorreu um erro ao salvar a notícia com Título {request.Titulo}.");
            }
            else
            {
                result.AddMessage("Notícia gravada com sucesso.");
            }

            return result;
        }
    }
}