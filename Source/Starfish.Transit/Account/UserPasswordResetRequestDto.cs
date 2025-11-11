namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data transfer object for user password reset.
/// </summary>
public class UserPasswordResetRequestDto
{
    /// <summary>
    /// Gets or sets the password reset operation token.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the new password for the user.
    /// </summary>
    public string Password { get; set; }
}