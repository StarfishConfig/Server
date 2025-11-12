using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Command to remove members from a team.
/// </summary>
/// <param name="teamId"></param>
/// <param name="userIds"></param>
internal sealed class TeamMemberRemoveCommand(long teamId, List<long> userIds)
    : Command<long>(teamId)
{
    /// <summary>
    /// Gets the team identifier.
    /// </summary>
    public long TeamId => Item1;

    /// <summary>
    /// Gets or sets the user identifiers to be removed from the team.
    /// </summary>
    public List<long> UserIds { get; init; } = userIds;
}