namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Criteria for querying teams.
/// </summary>
public class TeamCriteriaDto
{
    /// <summary>
    /// Keyword to filter teams by name or description.
    /// </summary>
    public string Keyword { get; set; }
}
