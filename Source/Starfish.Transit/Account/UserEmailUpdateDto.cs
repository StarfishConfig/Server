namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data Transfer Object for updating a user's email.
/// </summary>
public class UserEmailUpdateDto
{
    /// <summary>
    /// Gets or sets the new email address of the user.
    /// </summary>
    public string Email { get; set; }
}