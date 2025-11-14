using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to increase the project count for a team.
/// </summary>
/// <param name="id"></param>
public class TeamProjectCountIncreaseCommand(long id) : Command<long>(id)
{
    /// <summary>
    /// Gets the team identifier which the project belongs to.
    /// </summary>
    public long EntryId => Item1;
}