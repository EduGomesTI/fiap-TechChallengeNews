using MediatR;
using Microsoft.Extensions.Logging;
using News.Application.Abstractions;
using News.Domain.Entities;
using News.Domain.Enuns;
using News.Domain.Messages;
using News.Domain.Repositories;

namespace News.Application.News.Commands
{
    public sealed class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand, DeleteNewsResponse>
    {
        private readonly ILogger<DeleteNewsCommandHandler> _logger;
        private readonly INewsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;

        public DeleteNewsCommandHandler(ILogger<DeleteNewsCommandHandler> logger, INewsRepository repository, IUnitOfWork unitOfWork, IMessageService messageService)
        {
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _messageService = messageService;
        }

        public async Task<DeleteNewsResponse> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retorna a notícia que será excluída.");
            var noticia = await _repository.GetByIdAsync(request.Id);

            DeleteNewsResponse result = new();

            if (noticia == null)
            {
                result.AddMessage($"Não foi encontrada uma notícia com o Id {request.Id}.");
                return result;
            }

            _logger.LogInformation("Exclui uma notícia.");
            await _repository.DeleteAsync(noticia);

            var returnOfSaveChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _messageService.SendAsync(noticia);

            if (returnOfSaveChanges == 0)
            {
                result.AddMessage($"Ocorreu um erro ao excluir a notícia com Id {noticia.Id}.");
            }
            else
            {
                result.AddMessage("Notícia excluída com sucesso.");
            }

            return result;
        }
    }
}