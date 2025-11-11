using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Request to authenticate a user with an external provider.
/// </summary>
/// <param name="Provider"></param>
/// <param name="OpenId"></param>
internal record AuthenticateWithAuthProviderRequest(string Provider, string OpenId) : IRequest<TokenGrantResultDto>;