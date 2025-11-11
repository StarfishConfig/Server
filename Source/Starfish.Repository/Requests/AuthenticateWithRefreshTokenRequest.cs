using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Request to authenticate using a refresh token.
/// </summary>
/// <param name="Token"></param>
internal record AuthenticateWithRefreshTokenRequest(string Token) : IRequest<TokenGrantResultDto>;
