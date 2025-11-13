using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Application service for managing teams.
/// </summary>
internal sealed class TeamApplicationService : BaseApplicationService, ITeamApplicationService
{
    public Task<List<TeamListDto>> ListAsync(TeamCriteriaDto criteria, int skip, int take, CancellationToken cancellationToken = default)
    {
        return Bus.RequestAsync(new TeamListQueryRequest(criteria, skip, take), cancellationToken)
                  .ContinueWith(task =>
                  {
                      task.WaitAndUnwrapException(cancellationToken);
                      var entities = task.Result;
                      return TypeAdapter.ProjectedAs<List<TeamListDto>>(entities);
                  }, cancellationToken);
    }

    public Task<int> CountAsync(TeamCriteriaDto criteria, CancellationToken cancellationToken = default)
    {
        return Bus.RequestAsync(new TeamCountQueryRequest(criteria), cancellationToken)
                  .ContinueWith(task =>
                  {
                      task.WaitAndUnwrapException(cancellationToken);
                      return task.Result;
                  }, cancellationToken);
    }

    public Task<TeamDetailDto> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return Bus.RequestAsync(new TeamDetailQueryRequest(id), cancellationToken)
                  .ContinueWith(task =>
                  {
                      task.WaitAndUnwrapException(cancellationToken);
                      var entity = task.Result;
                      return TypeAdapter.ProjectedAs<TeamDetailDto>(entity);
                  }, cancellationToken);
    }

    public Task<long> CreateAsync(TeamCreateDto data, CancellationToken cancellationToken = default)
    {
        var command = TypeAdapter.ProjectedAs<TeamCreateCommand>(data);
        return Bus.SendAsync<TeamCreateCommand, long>(command, cancellationToken)
                  .ContinueWith(task =>
                  {
                      task.WaitAndUnwrapException(cancellationToken);
                      return task.Result;
                  }, cancellationToken);
    }

    public Task UpdateAsync(long id, TeamUpdateDto data, CancellationToken cancellationToken = default)
    {
        var command = new TeamUpdateCommand(id);
        command = TypeAdapter.ProjectedAs(data, command);
        return Bus.SendAsync(command, cancellationToken);
    }

    public Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var command = new TeamDeleteCommand(id);
        return Bus.SendAsync(command, cancellationToken);
    }

    public Task TransferAsync(long id, TeamTransferDto data, CancellationToken cancellationToken = default)
    {
        var command = new TeamTransferCommand(id)
        {
            UserId = data.UserId
        };
        return Bus.SendAsync(command, cancellationToken);
    }

    public Task<List<TeamMemberInfoDto>> GetMemberListAsync(long teamId, TeamMemberCriteriaDto criteria, int skip, int take, CancellationToken cancellationToken = default)
    {
        return Bus.RequestAsync(new TeamMemberListQueryRequest(teamId, criteria, skip, take), cancellationToken)
                  .ContinueWith(task =>
                  {
                      task.WaitAndUnwrapException(cancellationToken);
                      var entities = task.Result;
                      return TypeAdapter.ProjectedAs<List<TeamMemberInfoDto>>(entities);
                  }, cancellationToken);
    }

    public Task<int> GetMemberCountAsync(long teamId, TeamMemberCriteriaDto criteria, CancellationToken cancellationToken = default)
    {
        return Bus.RequestAsync(new TeamMemberCountQueryRequest(teamId, criteria), cancellationToken);
    }

    public Task AppendMemberAsync(long teamId, List<long> userIds, CancellationToken cancellationToken = default)
    {
        var command = new TeamMemberAppendCommand(teamId, userIds);
        return Bus.SendAsync(command, cancellationToken);
    }

    public Task RemoveMemberAsync(long teamId, List<long> userIds, CancellationToken cancellationToken = default)
    {
        var command = new TeamMemberRemoveCommand(teamId, userIds)
        {
            Reason = "remove"
        };
        return Bus.SendAsync(command, cancellationToken);
    }

    public Task QuitAsync(long teamId, CancellationToken cancellationToken = default)
    {
        var command = new TeamMemberRemoveCommand(teamId, [User.GetUserIdOfInt64()])
        {
            Reason = "quit"
        };
        return Bus.SendAsync(command, cancellationToken);
    }
}