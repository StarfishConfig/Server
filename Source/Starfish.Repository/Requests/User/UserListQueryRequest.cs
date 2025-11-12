using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Request to get a list of users based on criteria with pagination.
/// </summary>
/// <param name="Criteria"></param>
/// <param name="Skip"></param>
/// <param name="Take"></param>
internal sealed record UserListQueryRequest(UserCriteriaDto Criteria, int Skip, int Take) : IRequest<List<User>>;