using MassTransit;
using SendMail.Entities;

namespace SendMail.Events
{
    public class EmailSended : IConsumer<Noticia>
    {
        public Task Consume(ConsumeContext<Noticia> context)
        {
            Console.WriteLine($"Email sended with the news {context.Message.Titulo}");

            return Task.CompletedTask;
        }
    }
}