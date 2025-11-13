namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines an event that is triggered when the code of a <see cref="DictionaryRoot"/> is changed.
/// </summary>
public class DictionaryCodeChangedEvent : EntityPropertyChangedEvent<long, string>
{
    internal DictionaryCodeChangedEvent(long id, string oldValue, string newValue)
        : base(id, oldValue, newValue)
    {
    }
}
