using Microsoft.Extensions.Logging;
using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Application;

internal sealed class LoggingEventSubscriber(ILoggerFactory logger) : IHandler<UserAuthSucceedEvent>,
                                                                      IHandler<UserAuthFailedEvent>
{
    private readonly ILogger<LoggingEventSubscriber> _logger = logger.CreateLogger<LoggingEventSubscriber>();

    public async Task HandleAsync(UserAuthSucceedEvent message, MessageContext context, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Received UserAuthSucceedEvent {Message}", message);
    }

    public async Task HandleAsync(UserAuthFailedEvent message, MessageContext context, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Received UserAuthFailedEvent {Message}", message);
    }
}