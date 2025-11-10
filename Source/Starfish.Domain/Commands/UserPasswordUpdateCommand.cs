namespace Nerosoft.Starfish.Domain;

/// <summary>
/// The user password change command.
/// </summary>
/// <remarks>
/// This command is used to change a user's password, either through a reset or an update.
/// </remarks>
internal class UserPasswordUpdateCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserPasswordUpdateCommand"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="password"></param>
    /// <param name="actionType"></param>
    public UserPasswordUpdateCommand(long userId, string password, string actionType)
    {
        UserId = userId;
        Password = password;
        ActionType = actionType;
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
    /// Gets or sets the type of password change: "reset", or "change".
    /// </summary>
    public string ActionType { get; set; }
}