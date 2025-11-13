namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Event triggered when the owner of a team changes.
/// </summary>
public class TeamOwnerChangedEvent : EntityPropertyChangedEvent<long, long>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TeamOwnerChangedEvent"/> class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public TeamOwnerChangedEvent(long id, long oldValue, long newValue)
        : base(id, oldValue, newValue)
    {
    }
}