namespace Nerosoft.Starfish.Transit;

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