using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Euonia.Modularity;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Defines the repository module for the Starfish application.
/// </summary>
[DependsOn(typeof(DomainServiceModule))]
public class RepositoryModule : ModuleContextBase
{
    private const string CONNECTION_STRING_PATTERN = @"^(?<dbtype>(?:\w|\-)+):\/\/(?<conn>.*)";

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
    public override void AheadConfigureServices(ServiceConfigurationContext context)
    {
        Configure<UnitOfWorkOptions>(options =>
        {
            options.IsTransactional = false;
        });
    }

    /// <inheritdoc />
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddContextProvider();
        context.Services.AddUnitOfWork();

        context.Services.AddKeyedSingleton<IModelBuilder, IdentityModelBuilder>("IdentityModelBuilder");
        context.Services.AddKeyedSingleton<IModelBuilder, ProjectModelBuilder>("ProjectModelBuilder");

        context.Services.AddDbContextFactory<IdentityDataContext>((_, options) =>
        {
            var connectionString = Configuration.GetConnectionString("IdentityConnection");
            ConfigureDatabaseType(options, connectionString);
        });

        context.Services.AddDbContextFactory<ProjectDataContext>((_, options) =>
        {
            var connectionString = Configuration.GetConnectionString("ProjectConnection");
            ConfigureDatabaseType(options, connectionString);
        });
    }

    private static void ConfigureDatabaseType(DbContextOptionsBuilder options, string connectionString)
    {
        var match = Regex.Match(connectionString, CONNECTION_STRING_PATTERN);
        if (!match.Success)
        {
            throw new ArgumentException("Invalid connection string format.");
        }

        var databaseType = match.Groups["dbtype"].Value;
        var connection = match.Groups["conn"].Value;

        if (_databaseTypeAlias.TryGetValue(databaseType, out var dbType))
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    options.UseSqlServer(connection, builder =>
                    {
                        builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(2), null);
                        builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                    });
                    break;
                case DatabaseType.MySql:
                    options.UseMySql(connection, ServerVersion.AutoDetect(connection), builder =>
                    {
                        builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(2), null);
                        builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                    });
                    break;
                case DatabaseType.PostgreSql:
                    options.UseNpgsql(connection, builder =>
                    {
                        builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(2), null);
                        builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                    });
                    break;
                case DatabaseType.Sqlite:
                    options.UseSqlite(connection, builder =>
                    {
                        builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                    });
                    break;
                case DatabaseType.MongoDb:
                    options.UseMongoDB(connection, "");
                    break;
                case DatabaseType.InMemory:
                    options.UseInMemoryDatabase("Starfish");
                    break;
                default:
                    throw new NotSupportedException($"Database type '{databaseType}' is not supported.");
            }
        }
        else
        {
            throw new ArgumentException($"Unknown database type alias: '{databaseType}'");
        }
    }
}