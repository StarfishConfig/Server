using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// The data context for support-related data.
/// </summary>
internal class SupportDataContext : DataContextWithBus<SupportDataContext>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SupportDataContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="provider"></param>
    public SupportDataContext(DbContextOptions<SupportDataContext> options, ILazyServiceProvider provider)
        : base(options, provider)
    {
    }
}