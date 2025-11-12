using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Request to get the count of teams matching a keyword.
/// </summary>
internal sealed record TeamCountQueryRequest(TeamCriteriaDto Criteria) : IRequest<int>;