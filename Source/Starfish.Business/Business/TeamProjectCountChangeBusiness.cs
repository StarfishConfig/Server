using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Threading;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

internal sealed class TeamProjectCountChangeBusiness : CommandObjectBase<TeamProjectCountChangeBusiness>, IDomainService
{
    private ITeamRepository _repository;
    private ILockFactory _lock;

    private ITeamRepository Repository => _repository ??= LazyServiceProvider.GetService<ITeamRepository>();
    private ILockFactory Lock => _lock ??= LazyServiceProvider.GetService<ILockFactory>();

    [FactoryExecute]
    public async Task ExecuteAsync(long teamId, int changeType, CancellationToken cancellationToken = default)
    {
        if (teamId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(teamId));
        }
        if (changeType == 0)
        {
            return;
        }
        var lockKey = $"TeamProjectCountChangeBusiness_Team_{teamId}";

        using (await Lock.TryAcquireLockAsync(lockKey, TimeSpan.FromSeconds(5), cancellationToken))
        {
            var team = await Repository.GetAsync(teamId, true, cancellationToken);
            if (team == null)
            {
                throw new NotFoundException();
            }
            team.SetProjectCount(team.ProjectCount + changeType);
            await Repository.UpdateAsync(team, true, cancellationToken);
        }
    }
}
