using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Defines the application service for authentication-related operations.
/// </summary>
public interface IAuthApplicationService : IApplicationService
{
    /// <summary>
    /// Grants authentication based on the provided request.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AuthResultDto> GrantAsync(AuthRequestDto data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes the authentication tokens using the provided refresh token.
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AuthResultDto> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Revokes the authentication associated with the given token identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeAsync(string id, CancellationToken cancellationToken = default);
}
