using News.Application.News.Commands;
using News.Application.News.Queries;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace IntegrationTests
{
    public class AddNewsIntegrationTests : IClassFixture<NewsApplicationFactory>
    {
        private readonly NewsApplicationFactory _addWebApplicationFactory;
        private HttpClient _client;

        public AddNewsIntegrationTests(NewsApplicationFactory webApplicationFactory)
        {
            _addWebApplicationFactory = webApplicationFactory;
            _client = _addWebApplicationFactory.CreateClient();
        }

        [Fact]
        public async void Should_Add_News_Correcty()
        {
            //Arrange
            var request = new AddNewsCommand("Título", "Descrição", DateTime.Now, "Autor");
            var body = JsonSerializer.Serialize(request);
            var stringContent = new StringContent(body, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("/api/Noticias", stringContent, new CancellationToken());
            var countLines = await _client.GetFromJsonAsync<List<GetAllNewsResponse>>("api/Noticias");

            //Assert
            Assert.Equal(1, countLines.Count);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}