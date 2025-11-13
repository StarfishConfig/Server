namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines an event that is triggered when the name of a <see cref="DictionaryRoot"/> is changed.
/// </summary>
public class DictionaryNameChangedEvent : EntityPropertyChangedEvent<long, string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DictionaryNameChangedEvent"/> class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    internal DictionaryNameChangedEvent(long id, string oldValue, string newValue)
        : base(id, oldValue, newValue)
    {
    }
}
