using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines an event that is triggered when a new member joins a team.
/// </summary>
public class TeamMemberJoinedEvent : DomainEvent
{
    private readonly List<long> _userIds = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="TeamMemberJoinedEvent"/> class.
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="userIds"></param>
    internal TeamMemberJoinedEvent(long teamId, params long[] userIds)
    {
        TeamId = teamId;
        _userIds.AddRange(userIds);
    }

    /// <summary>
    /// Gets or sets the identifier of the team that the member has joined.
    /// </summary>
    public long TeamId { get; init; }

    /// <summary>
    /// Gets the identifier of the user who has joined the team.
    /// </summary>
    public IReadOnlyList<long> UserIds => _userIds;
}