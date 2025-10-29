using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the event triggered when a user's phone number is changed.
/// </summary>
public class UserPhoneChangedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserPhoneChangedEvent"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public UserPhoneChangedEvent(long userId, string oldValue, string newValue)
    {
        UserId = userId;
        OldValue = oldValue;
        NewValue = newValue;
    }

    /// <summary>
    /// Gets the identifier of the user whose phone number has been changed.
    /// </summary>
    public long UserId { get; }

    /// <summary>
    /// Gets the old phone number.
    /// </summary>
    public string OldValue { get; }

    /// <summary>
    /// Gets the new phone number.
    /// </summary>
    public string NewValue { get; }
}