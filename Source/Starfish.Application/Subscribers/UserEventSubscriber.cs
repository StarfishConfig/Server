using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Handler for user authentication events.
/// </summary>
/// <param name="bus"></param>
internal sealed class UserEventSubscriber(IBus bus)
    : IHandler<UserAuthSucceedEvent>,
      IHandler<UserAuthFailedEvent>
{
    /// <summary>
    /// Handles user authentication success events.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(UserAuthSucceedEvent message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return bus.SendAsync(new UserFailureResetCommand(message.UserId), cancellationToken);
    }

    /// <summary>
    /// Handles user authentication failure events.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(UserAuthFailedEvent message, MessageContext context, CancellationToken cancellationToken = default)
    {
        if (string.Equals(message.AuthType, AuthenticationConstant.Provider.Username))
        {
            if (message.Data?.TryGetValue("Username", out var username) == true && !string.IsNullOrWhiteSpace(username))
            {
                return bus.SendAsync(new UserFailureIncreaseCommand(username), cancellationToken);
            }
        }

        {
            // preserve for other auth types in the future
        }
        return Task.CompletedTask;
    }
}