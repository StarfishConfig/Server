using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines an event that is triggered when a user account is unlocked.
/// </summary>
public class UserUnlockedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserUnlockedEvent"/> class.
    /// </summary>
    /// <param name="userId"></param>
    public UserUnlockedEvent(long userId)
    {
        UserId = userId;
    }

    /// <summary>
    /// Gets the identifier of the unlocked user.
    /// </summary>
    public long UserId { get; }
}