using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Request to get the count of team members based on criteria.
/// </summary>
/// <param name="TeamId"></param>
/// <param name="Criteria"></param>
internal sealed record TeamMemberCountQueryRequest(long TeamId, TeamMemberCriteriaDto Criteria) : IRequest<int>;