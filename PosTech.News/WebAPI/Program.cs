using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using News.Security.JWT;
using News.Infrastructure;
using News.Infrastructure.Options;
using News.Application;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Net.Mime;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIContagem", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
            "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n\r\n" +
            "Exemplo (informar sem as aspas): 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//Adiciona configurações do token
var tokenConfigurations = new TokenConfigurations();
new ConfigureFromConfigurationOptions<TokenConfigurations>(
    builder.Configuration.GetSection("TokenConfigurations"))
        .Configure(tokenConfigurations);

// Aciona a extensão que irá configurar o uso de
// autenticação e autorização via tokens
builder.Services.AddJwtSecurity(tokenConfigurations);

//Adiciona configurações do Banco de dados
builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

//Adiciona configurações do Azure Service Bus
builder.Services.ConfigureOptions<AzureServiceBusOptionsSetup>();

//Adicina as configurações do RabbitMQ no MassTransit
builder.Services.ConfigureOptions<MassTransitOptionsSetup>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var options = context.GetRequiredService<IOptions<MassTransitOptions>>().Value;

        cfg.Host(options.Server, "/", h =>
        {
            h.Username(options.User);
            h.Password(options.Password);
        });
    });
});

// configure DI for application services
//builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHealthChecks();

builder.Services.AddApplicationInsightsTelemetry();

builder.Services
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHealthChecks("/status-text");
app.UseHealthChecks("/status-json",
    new HealthCheckOptions()
    {
        ResponseWriter = async (context, report) =>
        {
            var result = JsonSerializer.Serialize(
                            new
                            {
                                currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                statusApplication = report.Status.ToString(),
                            });

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(result);
        }
    });

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{ }