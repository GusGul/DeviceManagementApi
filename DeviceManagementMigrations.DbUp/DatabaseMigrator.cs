using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class DatabaseMigrator
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseMigrator> _logger;

    public DatabaseMigrator(IConfiguration configuration, ILogger<DatabaseMigrator> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void MigrateDatabase()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsFromFileSystem("migrations")
            .LogToConsole()
            .JournalToPostgresqlTable("SchemaVersions", "public")
            .Build();

        var result = upgrader.PerformUpgrade();

        if (result.Successful)
        {
            _logger.LogInformation("Database migration completed successfully.");
        }
        else
        {
            _logger.LogError("Database migration failed: {0}", result.Error);
            throw new Exception($"Database migration failed: {result.Error}");
        }
    }
}