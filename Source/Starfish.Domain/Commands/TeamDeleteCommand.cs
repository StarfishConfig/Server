using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to delete a team.
/// </summary>
/// <param name="id"></param>
public sealed class TeamDeleteCommand(long id)
    : Command<long>(id)
{
    /// <summary>
    /// Gets the team identifier.
    /// </summary>
    public long TeamId => Item1;
}