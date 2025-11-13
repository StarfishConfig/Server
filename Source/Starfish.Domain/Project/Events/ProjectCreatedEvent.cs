using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the event that is raised when a new project is created.
/// </summary>
public class ProjectCreatedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectCreatedEvent"/> class.
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="name"></param>
    public ProjectCreatedEvent(long teamId, string name)
    {
        TeamId = teamId;
        Name = name;
    }

    /// <summary>
    /// Gets the team ID associated with the project.
    /// </summary>
    public long TeamId { get; }

    /// <summary>
    /// Gets the project name.
    /// </summary>
    public string Name { get; }
}