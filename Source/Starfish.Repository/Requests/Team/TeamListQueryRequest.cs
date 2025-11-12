using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Request to query a list of teams with pagination and keyword filtering.
/// </summary>
/// <param name="Criteria"></param>
/// <param name="Skip">Number of items to skip.</param>
/// <param name="Take">Number of items to take.</param>
internal sealed record TeamListQueryRequest(TeamCriteriaDto Criteria, int Skip, int Take) : IRequest<IReadOnlyList<Team>>;