using Nerosoft.Euonia.Modularity;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Defines the repository module for the Starfish application.
/// </summary>
[DependsOn(typeof(DomainServiceModule))]
internal class RepositoryModule : ModuleContextBase
{
    /// <summary>
    /// Defines a mapping of database type aliases to their corresponding DatabaseType enum values.
    /// </summary>
    private static readonly Dictionary<string, DatabaseType> _databaseTypeAlias = new()
    {
        { "mssql", DatabaseType.SqlServer },
        { "sqlserver", DatabaseType.SqlServer },
        { "mysql", DatabaseType.MySql },
        { "postgresql", DatabaseType.PostgreSql },
        { "postgre", DatabaseType.PostgreSql },
        { "pg", DatabaseType.PostgreSql },
        { "pgsql", DatabaseType.PostgreSql },
        { "postgres", DatabaseType.PostgreSql },
        { "sqlite", DatabaseType.Sqlite },
        { "mongodb", DatabaseType.MongoDb },
        { "mongo", DatabaseType.MongoDb },
        { "memory", DatabaseType.InMemory },
        { "inmemory", DatabaseType.InMemory },
        { "in-memory", DatabaseType.InMemory }
    };

    /// <inheritdoc />
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);
    }
}