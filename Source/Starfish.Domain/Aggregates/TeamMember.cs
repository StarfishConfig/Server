using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the team member entity.
/// </summary>
public sealed class TeamMember : Entity<long>, IHasCreateTime
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

    /// <summary>
    /// Creates a new <see cref="TeamMember"/> entity instance with specified user ID.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal static TeamMember Create(long userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(userId), Resources.IDS_ERROR_TEAM_MEMBER_USERID_INVALID);
        }

        var entity = new TeamMember
        {
            UserId = userId
        };
        return entity;
    }
}