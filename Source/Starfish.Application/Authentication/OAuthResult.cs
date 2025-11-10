namespace Nerosoft.Starfish.Application;

/// <summary>
/// Represents the result of an OAuth authentication process.
/// </summary>
public class OAuthResult
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the nickname of the user.
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Gets or sets the avatar URL of the user.
    /// </summary>
    public string AvatarUrl { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    public string Phone { get; set; }
}