using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace News.Infrastructure.Options
{
    public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
    {
        private readonly IConfiguration _configuration;
        private const string SectionName = "DatabaseOptions";

        public DatabaseOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(DatabaseOptions options)
        {
            var connectionStrings = _configuration.GetConnectionString("SqlServerConnection");

            options.ConnectionString = connectionStrings!;

            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}