namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Base DTO for user-related data transfer objects.
/// </summary>
public abstract class UserBaseDto
{
    /// <summary>
    /// Gets or sets the unique username.
    /// </summary>
    public virtual string Username { get; set; }
}