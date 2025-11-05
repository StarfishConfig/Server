using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Euonia.Repository.EfCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// The data context for support-related data.
/// </summary>
internal class SupportDataContext : DataContextBase<SupportDataContext>
{
    private readonly IModelBuilder _builder;

    /// <summary>
    /// Initializes a new instance of the <see cref="SupportDataContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="builder"></param>
    public SupportDataContext(DbContextOptions<SupportDataContext> options, [FromKeyedServices("SupportModelBuilder")] IModelBuilder builder)
        : base(options)
    {
        _builder = builder;
    }

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
