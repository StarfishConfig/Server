using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Repository for managing Team entities.
/// </summary>
internal sealed class TeamRepository : BaseRepository<IdentityDataContext, Team, long>, ITeamRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TeamRepository"/> class.
    /// </summary>
    /// <param name="provider"></param>
    public TeamRepository(IContextProvider provider)
        : base(provider)
    {
    }
}