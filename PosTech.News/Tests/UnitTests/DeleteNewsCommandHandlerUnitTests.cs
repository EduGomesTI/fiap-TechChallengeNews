using Microsoft.Extensions.Logging;
using Moq;
using News.Application.Abstractions;
using News.Application.News.Commands;
using News.Domain.Entities;
using News.Domain.Enuns;
using News.Domain.Messages;
using News.Domain.Repositories;

namespace UnitTests
{
    public class DeleteNewsCommandHandlerUnitTests
    {
        private readonly Mock<ILogger<DeleteNewsCommandHandler>> _logger;
        private readonly Mock<INewsRepository> _repository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IMessageService> _messageService;

        public DeleteNewsCommandHandlerUnitTests()
        {
            _logger = new Mock<ILogger<DeleteNewsCommandHandler>>();
            _repository = new Mock<INewsRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _messageService = new Mock<IMessageService>();
        }

        [Fact]
        [Trait("DeleteNews", "IdNotFound")]
        public void Should_return_error_message_if_Noticia_Id_Not_Found()
        {
            // Arrange
            var noticia = new Noticia();
            var request = new DeleteNewsCommand(0);
            var result = "Não foi encontrada uma notícia com o Id 0.";

            _repository.Setup(x => x.DeleteAsync(noticia)).Returns(Task.FromResult(noticia));
            _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
            _messageService.Setup(x => x.SendAsync(noticia)).Returns(Task.FromResult(noticia));

            // Act
            var DeleteNewsCommandHandler = new DeleteNewsCommandHandler(_logger.Object, _repository.Object, _unitOfWork.Object, _messageService.Object);

            var response = DeleteNewsCommandHandler.Handle(request, CancellationToken.None).Result;

            // Assert
            Assert.Equal(result, response.Messages.FirstOrDefault());
        }

        [Fact]
        [Trait("DeleteNews", "Success")]
        public void Should_return_message_with_success()
        {
            // Arrange
            var noticia = new Noticia(1, "Título", "Descrição", DateTime.Now, "Autor");
            const string result = "Notícia excluída com sucesso.";
            var request = new DeleteNewsCommand(1);

            _repository.Setup(x => x.DeleteAsync(noticia)).Returns(Task.FromResult(noticia));
            _repository.Setup(x => x.GetByIdAsync(1)).Returns(Task.FromResult(noticia));
            _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
            _messageService.Setup(x => x.SendAsync(noticia)).Returns(Task.FromResult(noticia));

            // Act
            var DeleteNewsCommandHandler = new DeleteNewsCommandHandler(_logger.Object, _repository.Object, _unitOfWork.Object, _messageService.Object);

            var response = DeleteNewsCommandHandler.Handle(request, CancellationToken.None).Result;

            // Assert
            Assert.Equal(result, response.Messages.FirstOrDefault());
        }
    }
}