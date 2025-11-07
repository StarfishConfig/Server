namespace Nerosoft.Starfish.Domain;

/// <summary>
/// The user password change command.
/// </summary>
/// <remarks>
/// This command is used to change a user's password, either through a reset or an update.
/// </remarks>
internal class UserPasswordChangeCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserPasswordChangeCommand"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="password"></param>
    /// <param name="changeType"></param>
    public UserPasswordChangeCommand(long userId, string password, string changeType)
    {
        UserId = userId;
        Password = password;
        ChangeType = changeType;
    }

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Gets or sets the new password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the type of password change: "reset", or "update".
    /// </summary>
    public string ChangeType { get; set; }
}
