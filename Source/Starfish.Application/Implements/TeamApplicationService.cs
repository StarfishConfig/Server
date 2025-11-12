using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

internal sealed class TeamApplicationService : BaseApplicationService, ITeamApplicationService
{
    public Task<List<TeamListDto>> FindAsync(TeamCriteriaDto criteria, int skip, int take, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync(TeamCriteriaDto criteria, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TeamDetailDto> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<long> CreateAsync(TeamCreateDto dto, CancellationToken cancellationToken = default)
    {
        var command = TypeAdapter.ProjectedAs<TeamCreateCommand>(dto);
        return Bus.SendAsync<TeamCreateCommand, long>(command, cancellationToken)
                  .ContinueWith(task =>
                  {
                      task.WaitAndUnwrapException(cancellationToken);
                      return task.Result;
                  }, cancellationToken);
    }

    public Task UpdateAsync(long id, TeamUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var command = new TeamUpdateCommand(id);
        command = TypeAdapter.ProjectedAs(dto, command);
        return Bus.SendAsync(command, cancellationToken);
    }

    public Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var command = new TeamDeleteCommand(id);
        return Bus.SendAsync(command, cancellationToken);
    }
}