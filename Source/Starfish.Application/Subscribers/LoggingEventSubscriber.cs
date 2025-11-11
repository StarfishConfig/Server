using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Application;

internal sealed class LoggingEventSubscriber : IHandler<UserAuthSucceedEvent>,
                                               IHandler<UserAuthFailedEvent>
{
    public Task HandleAsync(UserAuthSucceedEvent message, MessageContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task HandleAsync(UserAuthFailedEvent message, MessageContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}