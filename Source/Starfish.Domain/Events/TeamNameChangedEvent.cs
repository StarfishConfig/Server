namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines a domain event for when a team's name has changed.
/// </summary>
public class TeamNameChangedEvent : EntityPropertyChangedEvent<long, string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TeamNameChangedEvent"/> class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public TeamNameChangedEvent(long id, string oldValue, string newValue)
        : base(id, oldValue, newValue)
    {
    }
}