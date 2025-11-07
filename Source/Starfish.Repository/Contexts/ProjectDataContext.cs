using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// The data context for project-related data.
/// </summary>
internal sealed class ProjectDataContext : DataContextWithBus<ProjectDataContext>
{
    private readonly IModelBuilder _builder;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectDataContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="builder"></param>
    public ProjectDataContext(DbContextOptions<ProjectDataContext> options, ILazyServiceProvider provider)
        : base(options, provider)
    {
        _builder = provider.GetRequiredKeyedService<IModelBuilder>(ProjectModelBuilder.Key);
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _builder.Configure(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}