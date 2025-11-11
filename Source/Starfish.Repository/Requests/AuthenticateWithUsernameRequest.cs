using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// The authenticate with username request.
/// </summary>
/// <param name="Username"></param>
/// <param name="Password"></param>
internal record AuthenticateWithUsernameRequest(string Username, string Password) : IRequest<User>;
