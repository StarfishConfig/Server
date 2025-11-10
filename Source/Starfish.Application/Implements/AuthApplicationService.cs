using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
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

        try
        {
            var response = await Bus.SendAsync(request, cancellationToken);

        }
        catch (Exception exception)
        {
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<AuthResultDto> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        IRequest<AuthResultDto> request = new AuthenticateWithRefreshTokenRequest(refreshToken);
        var response = await Bus.SendAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task RevokeAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
