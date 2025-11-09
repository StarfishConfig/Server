using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Identity data context for managing identity-related entities.
/// </summary>
internal sealed class AccountDataContext : DataContextWithBus<AccountDataContext>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AccountDataContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="provider"></param>
    public AccountDataContext(DbContextOptions<AccountDataContext> options, ILazyServiceProvider provider)
        : base(options, provider)
    {
    }
}