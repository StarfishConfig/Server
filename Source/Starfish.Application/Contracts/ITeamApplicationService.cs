using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

public interface ITeamApplicationService : IApplicationService
{
    /// <summary>
    /// Finds teams by criteria.
    /// </summary>
    /// <param name="criteria"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<List<TeamListDto>> FindAsync(TeamCriteriaDto criteria, int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts teams by criteria.
    /// </summary>
    /// <param name="criteria"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<int> CountAsync(TeamCriteriaDto criteria, CancellationToken cancellationToken = default);

    ValueTask<TeamDetailDto>
}
