namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data Transfer Object for user profile information.
/// </summary>
public class UserProfileDto
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the unique username.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Gets or sets the nick name.
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Gets or sets the registration date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the time when password was changed.
    /// </summary>
    public DateTime? PasswordChangedTime { get; set; }
}