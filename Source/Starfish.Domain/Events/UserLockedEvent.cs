using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines an event that is triggered when a user account is locked.
/// </summary>
public class UserLockedEvent : DomainEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the locked user.
    /// </summary>
    public string Username { get; set; }
}