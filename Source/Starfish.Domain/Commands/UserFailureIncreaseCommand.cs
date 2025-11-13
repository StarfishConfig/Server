using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to increase user failure count.
/// </summary>
/// <param name="username"></param>
public sealed class UserFailureIncreaseCommand(string username) : Command<string>(username)
{
    /// <summary>
    /// Gets the username.
    /// </summary>
    public string Username => Item1;
}