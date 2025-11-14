using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Repository for managing Project entities.
/// </summary>
internal sealed class ProjectRepository : BaseRepository<PrimaryDataContext, Project, long>, IProjectRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectRepository"/> class.
    /// </summary>
    /// <param name="provider"></param>
    public ProjectRepository(IContextProvider provider)
        : base(provider)
    {
    }
}