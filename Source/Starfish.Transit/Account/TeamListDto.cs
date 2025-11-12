namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data Transfer Object for listing teams.
/// </summary>
public class TeamListDto : TeamBaseDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the team.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the object was created.
    /// </summary>
    public DateTime CreateTime { get; set; }
}
