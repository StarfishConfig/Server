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
    public Task HandleAsync(UserAuthSucceedEvent message, MessageContext context, CancellationToken cancellationToken = default)
    {
        return bus.SendAsync(new UserFailureResetCommand(message.UserId), cancellationToken);
    }

    public async Task HandleAsync(UserAuthFailedEvent message, MessageContext context, CancellationToken cancellationToken = default)
    {
        if (!string.Equals(message.AuthType, AuthenticationConstant.Provider.Username))
        {
            return;
        }

        var username = message.Data != null && message.Data.TryGetValue("Username", out var name)
            ? name
            : string.Empty;

        if (string.IsNullOrWhiteSpace(username))
        {
            return;
        }

        await bus.SendAsync(new UserFailureIncreaseCommand(username), cancellationToken);
    }
}