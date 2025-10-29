using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the event triggered when a user's email is changed.
/// </summary>
public class UserEmailChangedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserEmailChangedEvent"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public UserEmailChangedEvent(long userId, string oldValue, string newValue)
    {
        UserId = userId;
        OldValue = oldValue;
        NewValue = newValue;
    }

    /// <summary>
    /// Gets the identifier of the user whose email has been changed.
    /// </summary>
    public long UserId { get; }

    /// <summary>
    /// Gets the old email address.
    /// </summary>
    public string OldValue { get; }

    /// <summary>
    /// Gets the new email address.
    /// </summary>
    public string NewValue { get; }
}