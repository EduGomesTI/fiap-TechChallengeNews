using MediatR;
using Microsoft.Extensions.Logging;
using News.Application.Abstractions;
using News.Domain.Entities;
using News.Domain.Repositories;

namespace News.Application.News.Commands
{
    public sealed class AddNewsCommandHandler : IRequestHandler<AddNewsCommand, AddNewsResponse>
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

            AddNewsResponse result = new();

            if (request is null)
            {
                result.AddMessage("A requisição não pode ser nula.");

                return result;
            }

            if (request.Titulo.Length == 0)
            {
                result.AddMessage("O título não pode ser vazio.");

                return result;
            }

            if (request.Autor.Length == 0)
            {
                result.AddMessage("O autor não pode ser vazio.");

                return result;
            }

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