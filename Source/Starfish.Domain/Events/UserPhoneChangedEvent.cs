using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the event triggered when a user's phone number is changed.
/// </summary>
public class UserPhoneChangedEvent : EntityPropertyChangedEvent<long, string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserPhoneChangedEvent"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public UserPhoneChangedEvent(long userId, string oldValue, string newValue)
        : base(userId, oldValue, newValue)
    {
    }
}