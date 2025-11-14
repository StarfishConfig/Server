using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to append members to a team.
/// </summary>
/// <param name="teamId"></param>
/// <param name="userIds"></param>
public sealed class TeamMemberAppendCommand(long teamId, List<long> userIds)
    : Command<long>(teamId)
{
    /// <summary>
    /// Gets the team identifier.
    /// </summary>
    public long EntryId => Item1;

    /// <summary>
    /// Gets the user identifiers to be added as team members.
    /// </summary>
    public List<long> UserIds { get; init; } = userIds;
}