namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data Transfer Object for displaying user information.
/// </summary>
public abstract class UserDisplayDto : UserBaseDto
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Gets or sets the nickname.
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Gets or sets the creation time.
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Gets or sets the update time.
    /// </summary>
    public DateTime UpdateTime { get; set; }
}