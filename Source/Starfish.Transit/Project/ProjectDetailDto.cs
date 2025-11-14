namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data Transfer Object for detailed project information.
/// </summary>
public class ProjectDetailDto : ProjectBaseDto
{
    /// <summary>
    /// Gets or sets the team ID associated with the project.
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Gets or sets the team name associated with the project.
    /// </summary>
    public string TeamName { get; set; }
}