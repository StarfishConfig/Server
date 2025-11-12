using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to revoke a token.
/// </summary>
/// <param name="token"></param>
internal sealed class TokenRevokeCommand(string token)
    : Command<string>(token)
{
    /// <summary>
    /// Gets the token to be revoked.
    /// </summary>
    public string Token => Item1;
}