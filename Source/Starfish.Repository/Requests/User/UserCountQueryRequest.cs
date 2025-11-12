using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Request to get the count of users based on criteria.
/// </summary>
/// <param name="Criteria"></param>
internal sealed record UserCountQueryRequest(UserCriteriaDto Criteria) : IRequest<int>;