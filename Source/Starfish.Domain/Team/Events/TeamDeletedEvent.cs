using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Event representing the deletion of a team.
/// </summary>
public class TeamDeletedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TeamDeletedEvent"/> class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    public TeamDeletedEvent(long id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Gets or sets the identifier of the deleted team.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the deleted team.
    /// </summary>
    public string Name { get; set; }
}