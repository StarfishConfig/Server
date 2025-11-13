using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command for updating user information.
/// </summary>
/// <param name="id"></param>
public sealed class UserUpdateCommand(long id)
    : Command<long>(id)
{
    /// <summary>
    /// Gets the unique identifier of the user to be updated.
    /// </summary>
    public long UserId => Item1;

    /// <inheritdoc cref="User.Nickname"/>
    public string Nickname { get; set; }

    /// <inheritdoc cref="User.Phone"/>
    public string Phone { get; set; }

    /// <inheritdoc cref="User.Email"/>
    public string Email { get; set; }
}