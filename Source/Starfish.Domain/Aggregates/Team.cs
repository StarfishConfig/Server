using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines a team within the system.
/// </summary>
public class Team : Aggregate<long>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Team"/> class.
    /// </summary>
    private Team()
    {
    }

    private Team(string name)
        : this()
    {
        Name = name;
    }

    /// <summary>
    /// Gets or sets the name of the team.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the team.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the owner of the team.
    /// </summary>
    public long OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the number of members in the team.
    /// </summary>
    public int MemberCount { get; set; }

    /// <summary>
    /// Gets or sets the collection of team members.
    /// </summary>
    public HashSet<TeamMember> Members { get; set; }

    /// <summary>
    /// Creates a new team instance.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    internal static Team Create(string name, long ownerId)
    {
        var aggregate = new Team(name)
        {
            OwnerId = ownerId,
            MemberCount = 0,
            Members = []
        };
        aggregate.RaiseEvent(new TeamCreatedEvent { Name = name, OwnerId = ownerId });
        return aggregate;
    }

    /// <summary>
    /// Sets the name of the team.
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="ArgumentNullException"></exception>
    internal void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (string.Equals(name, Name, StringComparison.Ordinal))
        {
            return;
        }

        Name = name;
    }

    /// <summary>
    /// Sets the description of the team.
    /// </summary>
    /// <param name="description"></param>
    internal void SetDescription(string description)
    {
        if (string.Equals(description, Description, StringComparison.Ordinal))
        {
            return;
        }

        Description = description;
    }

    /// <summary>
    /// Adds a member to the team.
    /// </summary>
    /// <param name="member"></param>
    internal void AddMember(TeamMember member)
    {
        Members.Add(member);
        MemberCount = Members.Count;
        RaiseEvent(new TeamMemberJoinedEvent(Id, member.UserId));
    }

    /// <summary>
    /// Adds a member to the team by user ID.
    /// </summary>
    /// <param name="userId"></param>
    internal void AddMember(long userId)
    {
        Members ??= new HashSet<TeamMember>();
        if (Members.Any(t => t.UserId == userId))
        {
            return;
        }

        AddMember(TeamMember.Create(userId));
    }

    /// <summary>
    /// Removes a member from the team.
    /// </summary>
    /// <param name="member"></param>
    internal void RemoveMember(TeamMember member)
    {
        Members.Remove(member);
        MemberCount = Members.Count;
        RaiseEvent(new TeamMemberLeaveEvent(Id, member.UserId));
    }

    /// <summary>
    /// Removes a member from the team by user ID.
    /// </summary>
    /// <param name="userId"></param>
    internal void RemoveMember(long userId)
    {
        Members ??= new HashSet<TeamMember>();
        var member = Members.FirstOrDefault(t => t.UserId == userId);
        if (member == null)
        {
            return;
        }

        RemoveMember(member);
    }
}