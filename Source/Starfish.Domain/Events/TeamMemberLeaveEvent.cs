using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines an event that is triggered when a team member leaves a team.
/// </summary>
public class TeamMemberLeaveEvent : DomainEvent
{
    private readonly List<long> _userIds = new List<long>();

    /// <summary>
    /// Initializes a new instance of the <see cref="TeamMemberLeaveEvent"/> class.
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="userIds"></param>
    internal TeamMemberLeaveEvent(long teamId, params long[] userIds)
    {
        TeamId = teamId;
        _userIds.AddRange(userIds);
    }

    /// <summary>
    /// Gets the identifier of the team from which the member has left.
    /// </summary>
    public long TeamId { get; init; }

    /// <summary>
    /// Gets the identifier of the user who has left the team.
    /// </summary>
    public IReadOnlyList<long> UserIds => _userIds;

    /// <summary>
    /// Gets the reason for the member leaving the team.
    /// </summary>
    public string Reason { get; init; }
}