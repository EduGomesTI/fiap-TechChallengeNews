using Microsoft.Extensions.Options;

namespace SendEmail.Options
{
    public class MassTransitOptionsSetup : IConfigureOptions<MassTransitOptions>
    {
        private readonly IConfiguration _configuration;
        private const string SectionName = "MassTransitOptions";

        public MassTransitOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(MassTransitOptions options)
        {
            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}