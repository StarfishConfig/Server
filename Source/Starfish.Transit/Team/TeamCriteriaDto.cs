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

    /// <summary>
    /// Gets or sets a value indicating whether to filter teams by membership.
    /// </summary>
    public bool? Joined { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter teams by ownership.
    /// </summary>
    public bool? Owned { get; set; }
}