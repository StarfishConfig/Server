using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nerosoft.Euonia.Modularity;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Defines the repository module for the Starfish application.
/// </summary>
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
        context.Services.AddContextProvider()
               .AddUnitOfWork();

        context.Services.AddHostedService<DataSeeder>();

        context.Services.AddDbContextFactory<ProjectDataContext>((provider, options) => ConfigureDataContext("ProjectConnection", provider, options))
               .AddDbContextFactory<SupportDataContext>((provider, options) => ConfigureDataContext("SupportConnection", provider, options))
               .AddDbContextFactory<LoggingDataContext>((provider, options) => ConfigureDataContext("LoggingConnection", provider, options))
               .AddDbContextFactory<AccountDataContext>((provider, options) => ConfigureDataContext("AccountConnection", provider, options));

        context.Services.AddScoped<IUserRepository, UserRepository>()
               .AddScoped<ITokenRepository, TokenRepository>()
               .AddScoped<IProjectRepository, ProjectRepository>()
               .AddScoped<ITeamRepository, TeamRepository>()
               .AddScoped<IDictionaryRepository, DictionaryRepository>();
    }
    
    /// <summary>
    /// Configures the data context based on the provided connection string name.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="provider"></param>
    /// <param name="options"></param>
    /// <param name="seeding"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    private void ConfigureDataContext(string name, IServiceProvider provider, DbContextOptionsBuilder options, Func<DbContext, bool, CancellationToken, Task> seeding = null)
    {
        var connectionString = Configuration.GetConnectionString(name);

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        var match = Regex.Match(connectionString, CONNECTION_STRING_PATTERN);
        if (!match.Success)
        {
            throw new ArgumentException(Resources.IDS_ERROR_INVALID_CONN_STRING);
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
                    throw new NotSupportedException(string.Format(Resources.IDS_ERROR_DATABASE_TYPE_NOT_SUPPORTED, databaseType));
            }
        }
        else
        {
            throw new ArgumentException(string.Format(Resources.IDS_ERROR_UNKNOWN_DATABASE_TYPE_VANITY, databaseType));
        }

        if (seeding != null)
        {
            options.UseAsyncSeeding(seeding);
        }
    }
}