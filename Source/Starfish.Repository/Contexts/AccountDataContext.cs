using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Identity data context for managing identity-related entities.
/// </summary>
internal sealed class AccountDataContext : DataContextWithBus<AccountDataContext>
{
    private readonly IModelBuilder _builder;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountDataContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="provider"></param>
    public AccountDataContext(DbContextOptions<AccountDataContext> options, ILazyServiceProvider provider)
        : base(options, provider)
    {
        _builder = provider.GetRequiredKeyedService<IModelBuilder>(AccountModelBuilder.Key);
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _builder.Configure(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}