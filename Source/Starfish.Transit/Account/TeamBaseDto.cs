namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Defines the base data transfer object for team-related operations.
/// </summary>
public abstract class TeamBaseDto
{
    /// <summary>
    /// Gets or sets the name of the team.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the team.
    /// </summary>
    public string Description { get; set; }
}
