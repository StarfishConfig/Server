namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Criteria for querying team members.
/// </summary>
public class TeamMemberCriteriaDto
{
    /// <summary>
    /// Keyword to filter team members by name or other attributes.
    /// </summary>
    public string Keyword { get; set; }
}