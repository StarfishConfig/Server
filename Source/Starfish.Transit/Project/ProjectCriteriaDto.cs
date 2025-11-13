namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data transfer object for project criteria used in filtering projects.
/// </summary>
public class ProjectCriteriaDto
{
    /// <summary>
    /// Gets or sets the team ID to filter projects by team.
    /// </summary>
    public long TeamId { get; set; }
}