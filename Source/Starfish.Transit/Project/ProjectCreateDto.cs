namespace Nerosoft.Starfish.Transit;

/// <summary>
/// The data transfer object for creating a new project.
/// </summary>
public class ProjectCreateDto : ProjectEditDto
{
    /// <summary>
    /// Gets or sets the team ID associated with the project.
    /// </summary>
    public long TeamId { get; set; }
}