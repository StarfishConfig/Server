using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Handles team query requests.
/// </summary>
/// <param name="repository"></param>
internal sealed class TeamRequestHandler(ITeamRepository repository)
    : IHandler<TeamDetailQueryRequest>,
      IHandler<TeamListQueryRequest>,
    IHandler<TeamCountQueryRequest>
{
    /// <summary>
    /// Handles the team detail query request.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(TeamDetailQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return repository.GetAsync(message.Id, false, [nameof(Team.Members)], cancellationToken)
                         .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }

    public Task HandleAsync(TeamListQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        var specification = TeamSpecification.Matches(message.Keyword);
        specification &= TeamSpecification.HasMember(0);
        throw new NotImplementedException();
    }

    public Task HandleAsync(TeamCountQueryRequest message, MessageContext context, CancellationToken cancellationToken = default)
    {
        var specification = TeamSpecification.Matches(message.Keyword);
        specification &= TeamSpecification.HasMember(0);
        var predicate = specification.Satisfy();

        return repository.CountAsync(predicate, cancellationToken)
                         .ContinueWith(task => context.Response(task.Result), cancellationToken);
    }
}
