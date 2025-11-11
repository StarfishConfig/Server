using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// The user password change command.
/// </summary>
/// <param name="userId"></param>
/// <param name="password"></param>
/// <param name="actionType">See the <see cref="Shared.UserPasswordChangeTypeConstant"/> to check available values.</param>
/// <remarks>
/// This command is used to change a user's password, either through a reset or an update.
/// </remarks>
internal class UserPasswordUpdateCommand(long userId, string password, string actionType)
    : Command<long>(userId)
{
    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public long UserId => Item1;

    /// <summary>
    /// Gets or sets the new password.
    /// </summary>
    public string Password { get; } = password;

    /// <summary>
    /// Gets or sets the type of password change: "reset", or "change".
    /// </summary>
    public string ActionType { get; } = actionType;
}