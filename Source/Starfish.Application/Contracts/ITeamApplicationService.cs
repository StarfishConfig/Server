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
    Task<List<TeamListDto>> FindAsync(TeamCriteriaDto criteria, int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts teams by criteria.
    /// </summary>
    /// <param name="criteria"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CountAsync(TeamCriteriaDto criteria, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets team detail by identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TeamDetailDto> GetAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new team.
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> CreateAsync(TeamCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing team.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateAsync(long id, TeamUpdateDto data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a team.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Transfers a team to another user.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task TransferAsync(long id, TeamTransferDto data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets team members by criteria.
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="criteria"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TeamMemberInfoDto>> FindMemberAsync(long teamId, TeamMemberCriteriaDto criteria, int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of team members by criteria.
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="criteria"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CountMemberAsync(long teamId, TeamMemberCriteriaDto criteria, CancellationToken cancellationToken = default);

    /// <summary>
    /// Appends members to a team.
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="userIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AppendMemberAsync(long teamId, List<long> userIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes members from a team.
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="userIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RemoveMemberAsync(long teamId, List<long> userIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// Quits the team.
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task QuitAsync(long teamId, CancellationToken cancellationToken = default);
}