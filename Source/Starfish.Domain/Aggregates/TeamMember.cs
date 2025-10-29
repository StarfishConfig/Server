using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the team member entity.
/// </summary>
public class TeamMember : Entity<long>, IHasCreateTime
{
    /// <summary>
    /// Default constructor for ORM.
    /// </summary>
    private TeamMember()
    {
    }

    /// <summary>
    /// Gets or sets the team identifier.
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Gets or sets the creation time of the team member.
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Gets or sets the team associated with the team member.
    /// </summary>
    public Team Team { get; set; }

    /// <summary>
    /// Gets or sets the user associated with the team member.
    /// </summary>
    public User User { get; set; }

    internal static TeamMember Create(long userId)
    {
        var entity = new TeamMember
        {
            UserId = userId
        };
        return entity;
    }
}