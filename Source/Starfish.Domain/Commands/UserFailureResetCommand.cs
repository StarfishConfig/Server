using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// The command to unlock a user account.
/// </summary>
internal sealed class UserFailureResetCommand(long id) : Command<long>(id)
{
    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public long UserId => Item1;
}