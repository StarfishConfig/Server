using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Subscriber for team-related events.
/// </summary>
internal sealed class TeamEventSubscriber(IBus bus)
    : IHandler<ProjectCreatedEvent>,
      IHandler<ProjectDeletedEvent>
{
    /// <summary>
    /// Handles the ProjectCreatedEvent.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(ProjectCreatedEvent message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return bus.SendAsync(new TeamProjectCountIncreaseCommand(message.TeamId), cancellationToken);
    }

    /// <summary>
    /// Handles the ProjectDeletedEvent.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(ProjectDeletedEvent message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return bus.SendAsync(new TeamProjectCountDecreaseCommand(message.TeamId), cancellationToken);
    }
}