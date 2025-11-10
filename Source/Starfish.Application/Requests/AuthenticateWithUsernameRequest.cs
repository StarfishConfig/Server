using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// The authenticate with username request.
/// </summary>
/// <param name="Username"></param>
/// <param name="Password"></param>
internal record AuthenticateWithUsernameRequest(string Username, string Password) : IRequest<AuthResultDto>;
