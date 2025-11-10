using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

internal class UserAuthSucceedEvent : ApplicationEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserAuthSucceedEvent"/> class.
    /// </summary>
    public UserAuthSucceedEvent()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAuthSucceedEvent"/> class.
    /// </summary>
    /// <param name="authType"></param>
    /// <param name="userId"></param>
    /// <param name="data"></param>
    public UserAuthSucceedEvent(string authType, long userId, Dictionary<string, string> data)
    {
        AuthType = authType;
        Data = data;
        UserId = userId;
    }

    /// <summary>
    /// Gets or sets the auth type.
    /// </summary>
    public string AuthType { get; set; }

    /// <summary>
    /// Gets or sets the additional data.
    /// </summary>
    public Dictionary<string, string> Data { get; set; }

    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the token issue time.
    /// </summary>
    public DateTime TokenIssueTime { get; set; }
}
