namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data Transfer Object for user profile information.
/// </summary>
public class UserProfileDto : UserDisplayDto
{
    /// <summary>
    /// Gets or sets the time when password was changed.
    /// </summary>
    public DateTime? PasswordChangedTime { get; set; }
}