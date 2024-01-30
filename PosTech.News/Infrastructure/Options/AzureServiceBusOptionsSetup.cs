using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace News.Infrastructure.Options
{
    public sealed class AzureServiceBusOptionsSetup : IConfigureOptions<AzureServiceBusOptions>
    {
        private readonly IConfiguration _configuration;
        private const string SectionName = "AzureServiceBusOptions";

        public AzureServiceBusOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(AzureServiceBusOptions options)
        {
            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}