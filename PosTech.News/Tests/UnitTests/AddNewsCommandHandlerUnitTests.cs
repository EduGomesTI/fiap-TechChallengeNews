using Microsoft.Extensions.Logging;
using Moq;
using News.Application.Abstractions;
using News.Application.News.Commands;
using News.Domain.Entities;
using News.Domain.Repositories;

namespace UnitTests
{
    public class AddNewsCommandHandlerUnitTests
    {
        private readonly Mock<ILogger<AddNewsCommandHandler>> _logger;
        private readonly Mock<INewsRepository> _repository;
        private readonly Mock<IUnitOfWork> _unitOfWork;

        public AddNewsCommandHandlerUnitTests()
        {
            _logger = new Mock<ILogger<AddNewsCommandHandler>>();
            _repository = new Mock<INewsRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        [Trait("AddNews", "RequestNull")]
        public void Should_return_error_message_if_request_is_null()
        {
            // Arrange
            var noticia = new Noticia();
            const string result = "A requisição não pode ser nula.";

            _repository.Setup(x => x.InsertAsync(noticia)).Returns(Task.FromResult(noticia));
            _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // Act
            var AddNewsCommandHandler = new AddNewsCommandHandler(_logger.Object, _repository.Object, _unitOfWork.Object);

            var response = AddNewsCommandHandler.Handle(null, CancellationToken.None).Result;

            // Assert
            Assert.Equal(result, response.Messages.FirstOrDefault());
        }

        [Fact]
        [Trait("AddNews", "TituloEmpty")]
        public void Should_return_error_message_if_Titulo_is_empty()
        {
            // Arrange
            var noticia = new Noticia();
            const string result = "O título não pode ser vazio.";
            var request = new AddNewsCommand
            {
                Titulo = string.Empty,
                Descricao = "Descrição",
                DataPublicacao = DateTime.Now,
                Autor = "Autor"
            };

            _repository.Setup(x => x.InsertAsync(noticia)).Returns(Task.FromResult(noticia));
            _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // Act
            var AddNewsCommandHandler = new AddNewsCommandHandler(_logger.Object, _repository.Object, _unitOfWork.Object);

            var response = AddNewsCommandHandler.Handle(request, CancellationToken.None).Result;

            // Assert
            Assert.Equal(result, response.Messages.FirstOrDefault());
        }

        [Fact]
        [Trait("AddNews", "AutorEmpty")]
        public void Should_return_error_message_if_Autor_is_empty()
        {
            // Arrange
            var noticia = new Noticia();
            const string result = "O autor não pode ser vazio.";
            var request = new AddNewsCommand
            {
                Titulo = "Título",
                Descricao = "Descrição",
                DataPublicacao = DateTime.Now,
                Autor = string.Empty
            };

            _repository.Setup(x => x.InsertAsync(noticia)).Returns(Task.FromResult(noticia));
            _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // Act
            var AddNewsCommandHandler = new AddNewsCommandHandler(_logger.Object, _repository.Object, _unitOfWork.Object);

            var response = AddNewsCommandHandler.Handle(request, CancellationToken.None).Result;

            // Assert
            Assert.Equal(result, response.Messages.FirstOrDefault());
        }

        [Fact]
        [Trait("AddNews", "DateNotSet")]
        public void Should_return_DateTime_Now_if_DataPublicao_is_not_set()
        {
            // Arrange
            var noticia = new Noticia(1, "Título", "Descrição", DateTime.MinValue, "Autor");

            // Act

            // Assert
            Assert.Equal(DateTime.Now.Date, noticia.DataPublicacao.Date);
        }

        [Fact]
        [Trait("AddNews", "Success")]
        public void Should_return_message_with_success()
        {
            // Arrange
            var noticia = new Noticia();
            const string result = "Notícia gravada com sucesso.";
            var request = new AddNewsCommand
            {
                Titulo = "Título",
                Descricao = "Descrição",
                DataPublicacao = DateTime.Now,
                Autor = "Autor"
            };

            _repository.Setup(x => x.InsertAsync(noticia)).Returns(Task.FromResult(noticia));
            _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // Act
            var AddNewsCommandHandler = new AddNewsCommandHandler(_logger.Object, _repository.Object, _unitOfWork.Object);

            var response = AddNewsCommandHandler.Handle(request, CancellationToken.None).Result;

            // Assert
            Assert.Equal(result, response.Messages.FirstOrDefault());
        }
    }
}