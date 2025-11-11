using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Request to query a list of teams with pagination and keyword filtering.
/// </summary>
/// <param name="skip"></param>
/// <param name="take"></param>
internal sealed class TeamListQueryRequest(int skip, int take) : TeamSearchRequest, IRequest<IReadOnlyList<Team>>
{
    /// <summary>
    /// Number of teams to skip.
    /// </summary>
    public int Skip { get; } = skip;

    /// <summary>
    /// Number of teams to take.
    /// </summary>
    public int Take { get; } = take;
}
