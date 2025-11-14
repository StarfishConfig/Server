using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to transfer a team.
/// </summary>
/// <param name="id"></param>
public sealed class TeamTransferCommand(long id) : Command<long>(id)
{
    /// <summary>
    /// Gets the team identifier.
    /// </summary>
    public long EntryId => Item1;

    /// <summary>
    /// Gets or sets the user identifier to transfer the team to.
    /// </summary>
    public required long UserId { get; init; }
}