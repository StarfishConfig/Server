namespace Nerosoft.Starfish.Transit;

public class ProjectCreateDto : ProjectEditDto
{
    /// <summary>
    /// Gets or sets the team ID associated with the project.
    /// </summary>
    public long TeamId { get; set; }
}