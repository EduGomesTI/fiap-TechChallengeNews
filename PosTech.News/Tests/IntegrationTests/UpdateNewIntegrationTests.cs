using News.Application.News.Commands;
using System.Net;
using System.Text;
using System.Text.Json;

namespace IntegrationTests
{
    public class UpdateNewsIntegrationTests : IClassFixture<NewsApplicationFactory>
    {
        private readonly NewsApplicationFactory _updateWebApplicationFactory;
        private HttpClient _client;

        public UpdateNewsIntegrationTests(NewsApplicationFactory webApplicationFactory)
        {
            _updateWebApplicationFactory = webApplicationFactory;
            _client = _updateWebApplicationFactory.CreateClient();
        }

        [Fact]
        public async void Should_Update_News_Correcty()
        {
            //Arrange
            var request = new AddNewsCommand("Título", "Descrição", DateTime.Now, "Autor");

            var body = JsonSerializer.Serialize(request);

            var stringContent = new StringContent(body, Encoding.UTF8, "application/json");

            await _client.PostAsync("/api/Noticias", stringContent);

            var updateRequest = new UpdateNewsCommand(1, " Novo Título", "Descrição", DateTime.Now, "Autor");

            var updateBody = JsonSerializer.Serialize(updateRequest);

            var updateStringContent = new StringContent(updateBody, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PutAsync("/api/Noticias", updateStringContent, new CancellationToken());

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}