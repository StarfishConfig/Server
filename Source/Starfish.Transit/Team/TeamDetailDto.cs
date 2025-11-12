namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data Transfer Object for detailed team information.
/// </summary>
public class TeamDetailDto : TeamBaseDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the team.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the number of members in the team.
    /// </summary>
    public int MemberCount { get; set; }

    /// <summary>
    /// Gets or sets the number of projects associated with the team.
    /// </summary>
    public int ProjectCount { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the object was created.
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the object was last updated.
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// Gets or sets the username of the creator of the team.
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the username of the last updater of the team.
    /// </summary>
    public string UpdatedBy { get; set; }
}