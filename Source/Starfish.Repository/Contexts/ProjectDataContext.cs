using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// The data context for project-related data.
/// </summary>
internal sealed class ProjectDataContext : DataContextWithBus<ProjectDataContext>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectDataContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="provider"></param>
    public ProjectDataContext(DbContextOptions<ProjectDataContext> options, ILazyServiceProvider provider)
        : base(options, provider)
    {
    }
}