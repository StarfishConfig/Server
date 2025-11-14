using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines a team within the system.
/// </summary>
public sealed class Team : Aggregate<long>, IAuditing
{
    /// <summary>
    /// Default constructor for ORM.
    /// </summary>
    private Team()
    {
        Register<TeamNameChangedEvent>(@event =>
        {
            Name = @event.NewValue;
        });
        Register<TeamOwnerChangedEvent>(@event =>
        {
            OwnerId = @event.OldValue;
        });
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
    /// Gets or sets the number of projects associated with the team.
    /// </summary>
    public int ProjectCount { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the team was created.
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the team was last updated.
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <inheritdoc />
    public string CreatedBy { get; set; }

    /// <inheritdoc />
    public string UpdatedBy { get; set; }

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
        aggregate.AppendMember(ownerId);
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

        RaiseEvent(new TeamNameChangedEvent(Id, Name, name));
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
    internal void AppendMember(TeamMember member)
    {
        Members.Add(member);
        MemberCount = Members.Count;
        RaiseEvent(new TeamMemberJoinedEvent(Id, member.UserId));
    }

    /// <summary>
    /// Adds a member to the team by user ID.
    /// </summary>
    /// <param name="userId"></param>
    internal void AppendMember(long userId)
    {
        Members ??= new HashSet<TeamMember>();
        if (Members.Any(t => t.UserId == userId))
        {
            return;
        }

        AppendMember(TeamMember.Create(userId));
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

    internal void AppendMembers(IEnumerable<long> userIds)
    {
        Members ??= [];

        userIds = userIds.Distinct().Where(id => !Members.Any(t => t.UserId == id)).ToList();
        if (userIds.Any())
        {
            return;
        }

        foreach (var userId in userIds)
        {
            Members.Add(TeamMember.Create(userId));
        }

        MemberCount = Members.Count;
        RaiseEvent(new TeamMemberJoinedEvent(Id, userIds.ToArray()));
    }

    internal void RemoveMembers(IEnumerable<long> userIds, string reason)
    {
        Members ??= [];

		if (userIds.Contains(OwnerId))
		{
			throw new ForbiddenException("Cannot remove the team owner from the team.");
		}

        var removingIds = userIds.Where(id => Members.Any(t => t.UserId == id)).ToList();
        if (!removingIds.Any())
        {
            return;
        }
        foreach (var userId in removingIds)
        {
            var member = Members.First(t => t.UserId == userId);
            Members.Remove(member);
        }
        MemberCount = Members.Count;
        RaiseEvent(new TeamMemberLeaveEvent(Id, [.. removingIds]) { Reason = reason });
    }

    /// <summary>
    /// Sets the project count for the team.
    /// </summary>
    /// <param name="count"></param>
    internal void SetProjectCount(int count)
    {
        if (count < 0)
        {
            count = 0;
        }

        ProjectCount = count;
    }

    /// <summary>
    /// Transfers ownership of the team to a new owner.
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="retainAsMember"></param>
    /// <exception cref="ArgumentNullException"></exception>
    internal void TransferOwnership(long ownerId, bool retainAsMember = true)
    {
        if (ownerId == 0)
        {
            throw new ArgumentNullException(nameof(ownerId));
        }

        if (ownerId == OwnerId)
        {
            return;
        }

        if (Members.All(t => t.UserId != ownerId))
        {
            throw new InvalidOperationException(Resources.IDE_ERROR_TEAM_OWNER_MUST_MEMBER);
        }

        RaiseEvent(new TeamOwnerChangedEvent(Id, OwnerId, ownerId));

        if (!retainAsMember)
        {
            RemoveMember(OwnerId);
        }
    }
}