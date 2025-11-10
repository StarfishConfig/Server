namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data Transfer Object for updating a user's phone information.
/// </summary>
public class UserPhoneUpdateDto
{
    /// <summary>
    /// Gets or sets the new phone number of the user.
    /// </summary>
    public string Phone { get; set; }
}