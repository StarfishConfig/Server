namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Event raised when a project's name is changed.
/// </summary>
public class ProjectNameChangedEvent : EntityPropertyChangedEvent<long, string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectNameChangedEvent"/> class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public ProjectNameChangedEvent(long id, string oldValue, string newValue)
        : base(id, oldValue, newValue)
    {
    }
}