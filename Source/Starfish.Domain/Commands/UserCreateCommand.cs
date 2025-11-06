using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// The user create command.
/// </summary>
internal class UserCreateCommand : Command
{
    /// <summary>
    /// Get or set the username.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Get or set the password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Get or set the email address.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Get or set the phone number.
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Get or set the nickname.
    /// </summary>
    public string Nickname { get; set; }
}