using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines an event that is triggered when a new member joins a team.
/// </summary>
public class TeamMemberJoinedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TeamMemberJoinedEvent"/> class.
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="userId"></param>
    internal TeamMemberJoinedEvent(long teamId, long userId)
    {
        TeamId = teamId;
        UserId = userId;
    }

    /// <summary>
    /// Gets or sets the identifier of the team that the member has joined.
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who has joined the team.
    /// </summary>
    public long UserId { get; set; }
}