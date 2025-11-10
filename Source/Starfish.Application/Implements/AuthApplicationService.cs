using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// The authentication application service implementation.
/// </summary>
internal class AuthApplicationService : BaseApplicationService, IAuthApplicationService
{
    /// <inheritdoc />
    public async Task<AuthResultDto> GrantAsync(AuthRequestDto data, CancellationToken cancellationToken = default)
    {
        IRequest<AuthResultDto> request = data.Provider?.ToLowerInvariant() switch
        {
            null or "" => throw new ArgumentNullException(nameof(data)),
            "username" => new AuthenticateWithUsernameRequest(data.Username, data.Password),
            "email" or "phone" => throw new NotImplementedException($"The provider '{data.Provider}' is not implemented."),
            "github" or "google" or "facebook" or "microsoft" => throw new NotImplementedException($"The provider '{data.Provider}' is not implemented."),
            _ => throw new NotSupportedException($"The provider '{data.Provider}' is not supported."),
        };

        var events = new List<ApplicationEvent>();

        try
        {
            var result = await Bus.SendAsync(request, cancellationToken);
            events.Add(new UserAuthSucceedEvent
            {
                AuthType = data.Provider,
                RefreshToken = result.RefreshToken,
                UserId = result.UserId,
                Username = result.Username,
                TokenIssueTime = DateTimeHelper.GetDateTimeFromUnixTime(result.IssueAt)
            });
            return result;
        }
        catch (Exception exception)
        {
            events.Add(new UserAuthFailedEvent
            {
                AuthType = data.Provider,
                Data = new Dictionary<string, string>
                {
                    { "Username", data.Username ?? string.Empty },
                    { "Password", data.Password != null ? "******" : string.Empty },
                },
                Error = exception.Message,
            });
            throw;
        }
        finally
        {
            if (events.Count > 0)
            {
                await Parallel.ForEachAsync(events, cancellationToken, async (@event, token) => await Bus.PublishAsync(@event, token));
            }
        }
    }

    /// <inheritdoc />
    public async Task<AuthResultDto> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        IRequest<AuthResultDto> request = new AuthenticateWithRefreshTokenRequest(refreshToken);

        var events = new List<ApplicationEvent>();
        try
        {
            var result = await Bus.SendAsync(request, cancellationToken);
            events.Add(new UserAuthSucceedEvent
            {
                AuthType = "refresh_token",
                RefreshToken = result.RefreshToken,
                UserId = result.UserId,
                Username = result.Username,
                TokenIssueTime = DateTimeHelper.GetDateTimeFromUnixTime(result.IssueAt)
            });

            events.Add(new TokenRefreshedEvent(refreshToken));

            return result;
        }
        finally
        {
            if (events.Count > 0)
            {
                await Parallel.ForEachAsync(events, cancellationToken, async (@event, token) => await Bus.PublishAsync(@event, token));
            }
        }
    }

    /// <inheritdoc />
    public Task RevokeAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
