using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Request to authenticate using a refresh token.
/// </summary>
/// <param name="RefreshToken"></param>
internal record AuthenticateWithRefreshTokenRequest(string RefreshToken) : IRequest<AuthResultDto>;
