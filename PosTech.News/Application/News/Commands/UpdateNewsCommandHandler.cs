using MediatR;
using Microsoft.Extensions.Logging;
using News.Application.Abstractions;
using News.Domain.Entities;
using News.Domain.Enuns;
using News.Domain.Messages;
using News.Domain.Repositories;

namespace News.Application.News.Commands
{
    public sealed class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, UpdateNewsResponse>
    {
        private readonly ILogger<UpdateNewsCommandHandler> _logger;
        private readonly INewsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;

        public UpdateNewsCommandHandler(ILogger<UpdateNewsCommandHandler> logger, INewsRepository repository, IUnitOfWork unitOfWork, IMessageService messageService)
        {
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _messageService = messageService;
        }

        public async Task<UpdateNewsResponse> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            //TODO: validar request

            UpdateNewsResponse result = new();

            if (request is null)
            {
                result.AddMessage("A requisição não pode ser nula.");

                return result;
            }

            if (request.Id == 0)
            {
                result.AddMessage("O Id da notícia não pode ser nulo.");

                return result;
            }

            _logger.LogInformation("Atualiza uma notícia.");
            Noticia noticia = new()
            {
                Id = request.Id,
                Titulo = request.Titulo,
                Descricao = request.Descricao,
                DataPublicacao = request.DataPublicacao,
                Autor = request.Autor
            };

            await _repository.UpdateAsync(noticia);

            var returnOfSaveChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _messageService.SendAsync(noticia);

            if (returnOfSaveChanges == 0)
            {
                result.AddMessage($"Ocorreu um erro ao atualizar a notícia com Título {request.Titulo}.");
            }
            else
            {
                result.AddMessage("Notícia atualizada com sucesso.");
            }

            return result;
        }
    }
}