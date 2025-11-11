using Microsoft.Extensions.Configuration;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Subscribes events which are related to token management.
/// </summary>
/// <param name="bus"></param>
/// <param name="configuration"></param>
internal sealed class TokenEventSubscriber(IBus bus, IConfiguration configuration)
    : IHandler<UserAuthSucceedEvent>,
      IHandler<UserAuthFailedEvent>,
      IHandler<TokenRefreshedEvent>
{
    /// <summary>
    /// Handles the user authentication succeeded event.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(UserAuthSucceedEvent message, MessageContext context, CancellationToken cancellationToken = default)
    {
        var lifeTimeDays = configuration.GetValue<int>("Auth:RefreshTokenLifeTimeDays", 180);

        var command = new TokenCreateCommand("refresh_token", message.UserId, message.RefreshToken)
        {
            Issued = message.TokenIssueTime,
            Expires = message.TokenIssueTime.AddDays(lifeTimeDays),
        };

        return bus.SendAsync(command, cancellationToken);
    }

    public Task HandleAsync(UserAuthFailedEvent message, MessageContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task HandleAsync(TokenRefreshedEvent message, MessageContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}