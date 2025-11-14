using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to update a team.
/// </summary>
/// <param name="id"></param>
public class TeamUpdateCommand(long id) : Command<long>(id)
{
    /// <summary>
    /// Gets the team identifier.
    /// </summary>
    public long EntryId => Item1;

    /// <inheritdoc cref="Team.Name"/>
    public string Name { get; set; }

    /// <inheritdoc cref="Team.Description"/>
    public string Description { get; set; }
}