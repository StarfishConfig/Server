using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Event published when a token is refreshed.
/// </summary>
internal class TokenRefreshedEvent : ApplicationEvent
{
    public TokenRefreshedEvent(string originToken)
    {
        OriginToken = originToken;
    }


    /// <summary>
    /// The origin access token.
    /// </summary>
    public string OriginToken { get; }
}
