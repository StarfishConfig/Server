using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to create a token.
/// </summary>
public sealed class TokenCreateCommand : Command
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenCreateCommand"/> class.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="subject"></param>
    /// <param name="token"></param>
    public TokenCreateCommand(string type, long subject, string token)
    {
        Type = type;
        Subject = subject;
        Token = token;
    }

    /// <summary>
    /// Gets or sets the token type.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the user identifier associated with the token.
    /// </summary>
    public long Subject { get; set; }

    /// <summary>
    /// Gets or sets the token string.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the token issue time.
    /// </summary>
    public DateTime Issued { get; init; }

    /// <summary>
    /// Gets or sets the token expiration time.
    /// </summary>
    public DateTime? Expires { get; init; }
}