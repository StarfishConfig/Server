using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Euonia.Repository.EfCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Identity data context for managing identity-related entities.
/// </summary>
internal sealed class AccountDataContext : DataContextBase<AccountDataContext>
{
    private readonly IModelBuilder _builder;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountDataContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="builder"></param>
    public AccountDataContext(DbContextOptions<AccountDataContext> options, [FromKeyedServices("AccountModelBuilder")] IModelBuilder builder)
        : base(options)
    {
        _builder = builder;
    }

    /// <inheritdoc/>
    protected override bool AutoSetEntryValues => true;

    /// <summary>
    /// Gets the DateTimeKind used for date and time values.
    /// </summary>
    protected override DateTimeKind DateTimeKind => DateTimeKind.Utc;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _builder.Configure(modelBuilder);
        modelBuilder.SetTombstoneQueryFilter();
        base.OnModelCreating(modelBuilder);
    }

    /// <inheritdoc/>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<DateTime>()
                            .HaveConversion<UniversalTimeConverter>();
        configurationBuilder.Properties<DateTime?>()
                            .HaveConversion<UniversalTimeConverter>();
    }
}