using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// Business logic for transferring team ownership.
/// </summary>
/// <param name="repository"></param>
internal sealed class TeamTransferBusiness(ITeamRepository repository)
    : CommandObjectBase<TeamTransferBusiness>, IDomainService
{
    /// <summary>
    /// Transfers the ownership of a team to another user.
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ForbiddenException"></exception>
    [FactoryExecute]
    public async Task ExecuteAsync(long teamId, long userId, CancellationToken cancellationToken = default)
    {
        var team = await repository.GetAsync(teamId, true, [nameof(Team.Members)], cancellationToken);
        if (team is null)
        {
            throw new NotFoundException();
        }

        // Ensure the current user is the owner of the team
        if (team.OwnerId != Identity.GetUserIdOfInt64())
        {
            throw new ForbiddenException();
        }

        team.TransferOwnership(userId);
        await repository.UpdateAsync(team, true, cancellationToken);
    }
}