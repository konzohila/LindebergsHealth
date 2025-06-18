using System;
using System.IO;
using System.Threading.Tasks;
using LindebergsHealth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Xunit;

namespace LindebergsHealth.Infrastructure.Tests.Integration
{
    public class DatabaseConnectionTests : IDisposable
    {
        private LindebergsHealthDbContext? _dbContext;

        /// <summary>
        /// Überprüft, ob eine Verbindung zur Azure SQL Datenbank für alle Umgebungen hergestellt werden kann
        /// </summary>
        [Theory]
        [InlineData("appsettings.Development.json")]
        [InlineData("appsettings.Staging.json")]
        [InlineData("appsettings.Production.json")]
        public async Task CanConnectToAzureSql(string settingsFile)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingsFile, optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();

            var configuration = configBuilder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
                Xunit.Assert.False(true, $"Connection string 'DefaultConnection' nicht gefunden in {settingsFile}");

            var options = new DbContextOptionsBuilder<LindebergsHealthDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _dbContext = new LindebergsHealthDbContext(options);

            bool canConnect = await _dbContext.Database.CanConnectAsync();
            Xunit.Assert.True(canConnect, $"Die Verbindung zur Azure SQL Datenbank ({settingsFile}) konnte nicht hergestellt werden.");
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
