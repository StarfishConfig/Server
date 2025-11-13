using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines an event that is triggered when a new user is created.
/// </summary>
public class UserCreatedEvent : DomainEvent
{
    public UserCreatedEvent(string username)
    {
        Username = username;
    }

    /// <summary>
    /// Gets or sets the unique identifier of the newly created user.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the source from which the user was created.
    /// </summary>
    public int Source { get; set; }
}