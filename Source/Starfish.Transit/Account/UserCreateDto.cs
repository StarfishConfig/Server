namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data Transfer Object for creating a new user.
/// </summary>
public class UserCreateDto
{
    /// <summary>
    /// Gets or sets the username of the new user.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password of the new user.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the nickname of the new user.
    /// </summary>
    public string Nickname { get; set; }
}