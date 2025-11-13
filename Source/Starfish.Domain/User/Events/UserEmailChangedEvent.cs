using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the event triggered when a user's email is changed.
/// </summary>
public class UserEmailChangedEvent : EntityPropertyChangedEvent<long, string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserEmailChangedEvent"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public UserEmailChangedEvent(long userId, string oldValue, string newValue)
        : base(userId, oldValue, newValue)
    {
    }
}