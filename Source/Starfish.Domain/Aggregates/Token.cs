using IdentityModel;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the token aggregate.
/// </summary>
public sealed class Token : Aggregate<long>
{
    /// <summary>
    /// Default constructor for ORM.
    /// </summary>
    private Token()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Token"/> class.
    /// </summary>
    /// <param name="type">The token type. <seealso cref="Type"/></param>
    /// <param name="key">The SHA256 hash of the token.</param>
    /// <param name="subject">The user id associated with the token.</param>
    /// <param name="issued">The token issued time.</param>
    /// <param name="expires">The token expiration time.</param>
    private Token(string type, string key, long subject, DateTime issued, DateTime? expires = null)
        : this()
    {
        Type = type;
        Key = key;
        Subject = subject;
        Issues = issued;
        Expires = expires;
    }

    /// <summary>
    /// Gets or sets the token type.
    /// </summary>
    /// <value>
    /// access_token, refresh_token
    /// </value>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the SHA256 hash of the token.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets the user identifier associated with the token.
    /// </summary>
    public long Subject { get; set; }

    /// <summary>
    /// Gets or sets the token issue time.
    /// </summary>
    public DateTime Issues { get; set; }

    /// <summary>
    /// Gets or sets the token expiration time.
    /// </summary>
    public DateTime? Expires { get; set; }

    /// <summary>
    /// Creates a new token aggregate.
    /// </summary>
    /// <param name="type">The token type. <seealso cref="Type"/></param>
    /// <param name="token">The original token string.</param>
    /// <param name="subject">The user id associated with the token.</param>
    /// <param name="issued">The token issued time.</param>
    /// <param name="expires">The token expiration time.</param>
    /// <returns></returns>
    internal static Token Create(string type, string token, long subject, DateTime issued, DateTime? expires = null)
    {
        var key = token.ToSha256();
        var entity = new Token(type, key, subject, issued, expires);
        return entity;
    }
}