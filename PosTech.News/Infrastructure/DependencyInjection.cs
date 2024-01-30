using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using News.Application.Abstractions;
using News.Domain.Messages;
using News.Domain.Repositories;
using News.Infrastructure.Data;
using News.Infrastructure.Data.Repositories;
using News.Infrastructure.Messages;
using News.Infrastructure.Options;

namespace News.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>((serviceprovider, dbContextOptionsBuilder) =>
            {
                var databaseOptions = serviceprovider.GetService<IOptions<DatabaseOptions>>()!.Value;

                dbContextOptionsBuilder.UseSqlServer(databaseOptions.ConnectionString, options =>
                {
                    options.EnableRetryOnFailure(databaseOptions.MaxRetryCount);

                    options.CommandTimeout(databaseOptions.CommandTimeOut);
                });

                dbContextOptionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnabledSensitiveDataLogging);

                dbContextOptionsBuilder.EnableDetailedErrors(databaseOptions.EnabledDetailedErrors);
            });

            services.AddScoped<INewsRepository, NoticiasRepository>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IMessageService, MessageService>();

            return services;
        }
    }
}