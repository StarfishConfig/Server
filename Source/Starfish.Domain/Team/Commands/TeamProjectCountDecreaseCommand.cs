using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to decrease the project count for a team.
/// </summary>
/// <param name="id"></param>
public class TeamProjectCountDecreaseCommand(long id)
    : Command<long>(id)
{
    /// <summary>
    /// Gets the team identifier which the project belongs to.
    /// </summary>
    public long EntryId => Item1;
}