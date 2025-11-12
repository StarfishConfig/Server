namespace Nerosoft.Starfish.Transit;

public class UserDetailDto : UserDisplayDto
{
    /// <summary>
    /// Gets or sets the number of failed access attempts.
    /// </summary>
    public int AccessFailedCount { get; set; }

    /// <summary>
    /// Gets or sets the lockout end time.
    /// </summary>
    public DateTime? LockoutEnd { get; set; }

    /// <summary>
    /// Gets or sets the source of the user account.
    /// </summary>
    /// <value>
    /// <see cref="Shared.UserCreationSource"/>
    /// <para>1 - Initial</para>
    /// <para>2 - Created by administrator</para>
    /// <para>3 - User self-registration</para>
    /// </value>
    public int Source { get; set; }

    /// <summary>
    /// Gets or sets the roles assigned to the user.
    /// </summary>
    public List<string> Roles { get; set; }
}