using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines an event that is triggered when a new team is created.
/// </summary>
public class TeamCreatedEvent : DomainEvent
{
    /// <summary>
    /// Gets or sets the name of the newly created team.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the owner of the newly created team.
    /// </summary>
    public long OwnerId { get; set; }
}