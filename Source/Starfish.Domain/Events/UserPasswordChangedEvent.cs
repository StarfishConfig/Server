using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the event triggered when a user's password is changed.
/// </summary>
public class UserPasswordChangedEvent : DomainEvent
{
    public UserPasswordChangedEvent(long userId, string changeType)
    {
        UserId = userId;
        ChangeType = changeType;
    }

    /// <summary>
    /// Gets the identifier of the user whose password has been changed.
    /// </summary>
    public long UserId { get; }

    /// <summary>
    /// Gets the type of password change (e.g., "Reset", "Update").
    /// </summary>
    public string ChangeType { get; }
}