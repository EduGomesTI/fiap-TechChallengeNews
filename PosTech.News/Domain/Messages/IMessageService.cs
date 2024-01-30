using News.Domain.Entities;

namespace News.Domain.Messages
{
    public interface IMessageService
    {
        Task SendAsync(Noticia noticia);
    }
}