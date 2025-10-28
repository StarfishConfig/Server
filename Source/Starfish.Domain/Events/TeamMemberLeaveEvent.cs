using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines an event that is triggered when a team member leaves a team.
/// </summary>
public class TeamMemberLeaveEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TeamMemberLeaveEvent"/> class.
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="userId"></param>
    internal TeamMemberLeaveEvent(long teamId, long userId)
    {
        TeamId = teamId;
        UserId = userId;
    }

    /// <summary>
    /// Gets or sets the identifier of the team from which the member has left.
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who has left the team.
    /// </summary>
    public long UserId { get; set; }
}