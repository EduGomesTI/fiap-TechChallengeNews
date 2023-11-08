using News.Application.News.Commands;
using News.Application.News.Queries;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace IntegrationTests
{
    public class DeleteNewsIntegrationTests : IClassFixture<NewsApplicationFactory>
    {
        private readonly NewsApplicationFactory _deleteWebApplicationFactory;
        private HttpClient _client;

        public DeleteNewsIntegrationTests(NewsApplicationFactory webApplicationFactory)
        {
            _deleteWebApplicationFactory = webApplicationFactory;
            _client = _deleteWebApplicationFactory.CreateClient();
        }

        [Fact]
        public async void Should_Delete_News_Correcty()
        {
            //Arrange
            var request = new AddNewsCommand("Título", "Descrição", DateTime.Now, "Autor");

            var body = JsonSerializer.Serialize(request);

            var stringContent = new StringContent(body, Encoding.UTF8, "application/json");

            await _client.PostAsync("/api/Noticias", stringContent);

            //Act
            var response = await _client.DeleteAsync("/api/Noticias/1", new CancellationToken());
            var countLines = await _client.GetFromJsonAsync<List<GetAllNewsResponse>>("api/Noticias");

            //Assert
            Assert.Equal(0, countLines.Count);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}