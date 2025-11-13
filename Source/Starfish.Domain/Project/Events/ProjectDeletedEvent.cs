using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Event raised when a project is deleted.
/// </summary>
public class ProjectDeletedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectDeletedEvent"/> class.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="teamId"></param>
    /// <param name="name"></param>
    public ProjectDeletedEvent(long projectId, long teamId, string name)
    {
        ProjectId = projectId;
        TeamId = teamId;
        Name = name;
    }

    /// <summary>
    /// Gets the ID of the deleted project.
    /// </summary>
    public long ProjectId { get; }

    /// <summary>
    /// Gets the team ID associated with the deleted project.
    /// </summary>
    public long TeamId { get; }

    /// <summary>
    /// Gets the name of the deleted project.
    /// </summary>
    public string Name { get; }
}