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
    public class UpdateNewsCommandHandlerUnitTests
    {
        private readonly Mock<ILogger<UpdateNewsCommandHandler>> _logger;
        private readonly Mock<INewsRepository> _repository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IMessageService> _messageService;

        public UpdateNewsCommandHandlerUnitTests()
        {
            _logger = new Mock<ILogger<UpdateNewsCommandHandler>>();
            _repository = new Mock<INewsRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _messageService = new Mock<IMessageService>();
        }

        [Fact]
        [Trait("UpdateNews", "RequestNull")]
        public void Should_return_error_message_if_request_is_null()
        {
            // Arrange
            var noticia = new Noticia();
            const string result = "A requisição não pode ser nula.";

            _repository.Setup(x => x.UpdateAsync(noticia)).Returns(Task.FromResult(noticia));
            _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
            _messageService.Setup(x => x.SendAsync(noticia)).Returns(Task.FromResult(noticia));

            // Act
            var UpdateNewsCommandHandler = new UpdateNewsCommandHandler(_logger.Object, _repository.Object, _unitOfWork.Object, _messageService.Object);

            var response = UpdateNewsCommandHandler.Handle(null, CancellationToken.None).Result;

            // Assert
            Assert.Equal(result, response.Messages.FirstOrDefault());
        }

        [Fact]
        [Trait("UpdateNews", "IdNull")]
        public void Should_return_error_message_if_request_Id_is_null()
        {
            // Arrange
            var noticia = new Noticia();
            var request = new UpdateNewsCommand();
            const string result = "O Id da notícia não pode ser nulo.";

            _repository.Setup(x => x.UpdateAsync(noticia)).Returns(Task.FromResult(noticia));
            _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
            _messageService.Setup(x => x.SendAsync(noticia)).Returns(Task.FromResult(noticia));

            // Act
            var UpdateNewsCommandHandler = new UpdateNewsCommandHandler(_logger.Object, _repository.Object, _unitOfWork.Object, _messageService.Object);

            var response = UpdateNewsCommandHandler.Handle(request, CancellationToken.None).Result;

            // Assert
            Assert.Equal(result, response.Messages.FirstOrDefault());
        }

        [Fact]
        [Trait("UpdateNews", "Success")]
        public void Should_return_message_with_success()
        {
            // Arrange
            var noticia = new Noticia();
            const string result = "Notícia atualizada com sucesso.";
            var request = new UpdateNewsCommand
            {
                Id = 1,
                Titulo = "Título",
                Descricao = "Descrição",
                DataPublicacao = DateTime.Now,
                Autor = "Autor"
            };

            _repository.Setup(x => x.InsertAsync(noticia)).Returns(Task.FromResult(noticia));
            _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
            _messageService.Setup(x => x.SendAsync(noticia)).Returns(Task.FromResult(noticia));

            // Act
            var UpdateNewsCommandHandler = new UpdateNewsCommandHandler(_logger.Object, _repository.Object, _unitOfWork.Object, _messageService.Object);

            var response = UpdateNewsCommandHandler.Handle(request, CancellationToken.None).Result;

            // Assert
            Assert.Equal(result, response.Messages.FirstOrDefault());
        }
    }
}