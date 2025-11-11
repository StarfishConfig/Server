namespace Nerosoft.Starfish.Application;

/// <summary>
/// Defines methods for authenticating users via external providers.
/// </summary>
public interface IAuthProvider
{
    /// <summary>
    /// Authorizes a user using the provided authorization code.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OAuthResult> AuthorizeAsync(string code, CancellationToken cancellationToken = default);
}