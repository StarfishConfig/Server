using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Data seeding operations.
/// </summary>
/// <param name="configuration"></param>
internal class DataSeeder(IConfiguration configuration, IServiceProvider provider, ILoggerFactory logger)
    : BackgroundService
{
    private readonly ILogger<DataSeeder> _logger = logger.CreateLogger<DataSeeder>();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var tasks = new List<Task>
            {
                SeedingAccountDataAsync(stoppingToken),
                SeedingDictionaryDataAsync(stoppingToken)
            };

            await Task.WhenAll(tasks);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An exception occurred");
        }
    }

    /// <summary>
    /// Seeds initial account data into the database.
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task SeedingAccountDataAsync(CancellationToken cancellationToken = default)
    {
        var context = provider.CreateScope().ServiceProvider.GetService<AccountDataContext>();

        var username = "admin";
        var password = "nerosoft.8888";

        var exists = await context.Set<User>().AnyAsync(u => u.Username == username, cancellationToken);
        if (exists)
        {
            return;
        }

        var user = User.Create(username, UserCreationSource.InitialImport);
        user.SetPassword(password);
        user.SetRoles("SA");

        await context.Set<User>().AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(true, cancellationToken);
    }

    private async Task SeedingDictionaryDataAsync(CancellationToken cancellationToken = default)
    {
        var context = provider.CreateScope().ServiceProvider.GetService<SupportDataContext>();

        var exists = await context.Set<DictionaryRoot>().AnyAsync(cancellationToken);
        if (exists)
        {
            return;
        }

        var roles = DictionaryRoot.Create("role", "System user roles");
        roles.SetItem(new Dictionary<string, string>
        {
            { "SU", "Super User" },
            { "SA", "System Administrator" },
            { "HD", "Helpdesk Operator" },
            { "US", "Normal User" }
        });

        var userSources = DictionaryRoot.Create("user_source", "User creation sources");
        userSources.SetItem(new Dictionary<string, string>
        {
            { $"{UserCreationSource.InitialImport}", "Initial data import" },
            { $"{UserCreationSource.AdminCreated}", "Created by administrator" },
            { $"{UserCreationSource.SelfRegistered}", "Self registration" }
        });


        await context.Set<DictionaryRoot>().AddRangeAsync([roles, userSources], cancellationToken);
        await context.SaveChangesAsync(true, cancellationToken);
    }
}