using MassTransit;
using Microsoft.Extensions.Options;
using SendEmail;
using SendEmail.Events;
using SendEmail.Options;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.ConfigureOptions<MassTransitOptionsSetup>();

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<MassTransitOptions>>().Value;

                cfg.Host(options.Server, "/", h =>
                {
                    h.Username(options.User);
                    h.Username(options.Password);
                });

                cfg.ReceiveEndpoint(options.Queue, e =>
                {
                    e.Consumer<EmailSended>();
                });

                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumer<EmailSended>();
        });

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();