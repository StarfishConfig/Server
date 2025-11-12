using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

internal sealed record TeamMemberListQueryRequest(long TeamId, TeamMemberCriteriaDto Criteria, int Skip, int Take)
    : IRequest<List<TeamMember>>;