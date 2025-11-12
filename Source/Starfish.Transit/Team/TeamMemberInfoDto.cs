namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data Transfer Object for team member information.
/// </summary>
public class TeamMemberInfoDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Gets or sets the username of the team member.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the nickname of the team member.
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the member joined the team.
    /// </summary>
    public DateTime JoinTime { get; set; }
}