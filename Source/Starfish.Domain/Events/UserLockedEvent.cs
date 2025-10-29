using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines an event that is triggered when a user account is locked.
/// </summary>
public class UserLockedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserLockedEvent"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="lockoutEnd"></param>
    public UserLockedEvent(long userId, DateTime lockoutEnd)
    {
        UserId = userId;
        LockoutEnd = lockoutEnd;
    }

    /// <summary>
    /// Gets the identifier of the locked user.
    /// </summary>
    public long UserId { get; }

    /// <summary>
    /// Gets the lockout end time.
    /// </summary>
    public DateTime LockoutEnd { get; }
}