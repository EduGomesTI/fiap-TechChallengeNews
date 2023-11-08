using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using News.Infrastructure.Data;
using Respawn;
using Testcontainers.MsSql;

namespace IntegrationTests
{
    public class NewsApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextOptionsDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                services.Remove(dbContextOptionsDescriptor!);

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlServer(_msSqlContainer.GetConnectionString());
                });
            });
        }

        private async Task CreateDatabase()
        {
            await using var connection = new SqlConnection(_msSqlContainer.GetConnectionString());

            await connection.OpenAsync();

            await using var command = connection.CreateCommand();

            command.CommandText = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='User' and xtype='U')
	                                CREATE TABLE [dbo].[User](Username VARCHAR(50) NOT NULL ,Password VARCHAR(MAX) NOT NULL,
	                                PRIMARY KEY (Username));";

            await command.ExecuteNonQueryAsync();

            command.CommandText = @"IF NOT EXISTS(SELECT 1 FROM dbo.[User] WHERE Username = 'usuario_fiap')
	                                INSERT [dbo].[User] values ('usuario_fiap', 'senha_fiap');";

            await command.ExecuteNonQueryAsync();

            command.CommandText = @"IF NOT EXISTS (SELECT *
                                    FROM sysobjects
                                    WHERE name='Noticia' and xtype='U')
	                                    CREATE TABLE Noticia
                                    (
	                                    Id INT PRIMARY KEY IDENTITY,
	                                    Titulo NVARCHAR(255) NOT NULL,
	                                    Descricao NVARCHAR(MAX) NOT NULL,
	                                    DataPublicacao DATETIME NOT NULL,
	                                    Autor NVARCHAR(255) NOT NULL
                                    );";

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }

        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();

            await CreateDatabase();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _msSqlContainer.StopAsync();
        }

        public async Task ClearDatabaseAsync()
        {
            using var connection = new SqlConnection(_msSqlContainer.GetConnectionString());

            await connection.OpenAsync();

            var respawner = await Respawner.CreateAsync(connection);

            await respawner.ResetAsync(connection);
        }
    }
}