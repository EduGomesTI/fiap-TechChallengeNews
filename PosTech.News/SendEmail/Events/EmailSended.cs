using MassTransit;
using News.Domain.Entities;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace SendEmail.Events
{
    public class EmailSended : IConsumer<Noticia>
    {
        public Task Consume(ConsumeContext<Noticia> context)
        {
            const string toEmail = "edugomesti@outlook.com";
            var subject = context.Message.Titulo;
            var body = new StringBuilder();
            body.AppendLine($"Autor: {context.Message.Autor}");
            body.AppendLine($"Data da Publicação da Notícia: {context.Message.DataPublicacao}");
            body.AppendLine($"Breve resumo da notícia: {context.Message.Descricao}");

            SendEmail(toEmail, subject, body.ToString());

            Console.WriteLine($"Email sended with the news {context.Message.Titulo}");

            return Task.CompletedTask;
        }

        private static void SendEmail(string toEmail, string subject, string body)
        {
            const string fromEmail = "dudolino@gmail.com";
            const string fromPassword = "uhii akat ottg ddme";

            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromEmail, fromPassword)
            };

            var message = new MailMessage(fromEmail, toEmail, subject, body);

            smtpClient.Send(message);
        }
    }
}