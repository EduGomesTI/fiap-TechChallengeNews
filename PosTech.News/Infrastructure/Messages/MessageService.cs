using MassTransit;
using Microsoft.Extensions.Options;
using News.Domain.Entities;
using News.Domain.Messages;
using News.Infrastructure.Options;

namespace News.Infrastructure.Messages
{
    public sealed class MessageService : IMessageService
    {
        private readonly IBus _bus;
        private readonly MassTransitOptions _options;

        public MessageService(IBus bus, IOptions<MassTransitOptions> options)
        {
            _bus = bus;
            _options = options.Value;
        }

        public async Task SendAsync(Noticia noticia)
        {
            var queueName = _options.Queue;

            var uri = new Uri($"queue:{queueName}");

            var endpoint = await _bus.GetSendEndpoint(uri);

            await endpoint.Send(noticia);
        }
    }
}