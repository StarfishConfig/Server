using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// The user create command.
/// </summary>
public class UserCreateCommand : Command
{
    /// <inheritdoc cref="User.Username"/>
    public string Username { get; set; }

    /// <summary>
    /// Get or set the password.
    /// </summary>
    public string Password { get; set; }

    /// <inheritdoc cref="User.Email"/>
    public string Email { get; set; }

    /// <inheritdoc cref="User.Phone"/>
    public string Phone { get; set; }

    /// <inheritdoc cref="User.Nickname"/>
    public string Nickname { get; set; }

    /// <inheritdoc cref="User.Source"/>
    public int Source { get; set; }
}