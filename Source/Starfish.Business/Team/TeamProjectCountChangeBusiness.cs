using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Threading;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// Business logic for changing the project count of a team.
/// </summary>
internal sealed class TeamProjectCountChangeBusiness : CommandObjectBase<TeamProjectCountChangeBusiness>
{
    private ITeamRepository Repository => LazyServiceProvider.GetService<ITeamRepository>();
    private ILockFactory Lock => LazyServiceProvider.GetService<ILockFactory>();

    [FactoryExecute]
    public async Task ExecuteAsync(long teamId, string changeType, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(teamId);

        var lockKey = $"TeamProjectCountChangeBusiness_Team_{teamId}";

        var handle = await Lock.TryAcquireLockAsync(lockKey, TimeSpan.FromSeconds(5), cancellationToken);

        await using (handle)
        {
            var team = await Repository.GetAsync(teamId, true, cancellationToken);
            if (team == null)
            {
                throw new NotFoundException();
            }

            switch (changeType)
            {
                case "increase":
                case "+":
                    team.SetProjectCount(team.ProjectCount + 1);
                    break;
                case "decrease":
                case "-":
                    team.SetProjectCount(team.ProjectCount - 1);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            await Repository.UpdateAsync(team, true, cancellationToken);
        }
    }
}