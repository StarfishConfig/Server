using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines an event that is triggered when a <see cref="DictionaryRoot"/> is deleted.
/// </summary>
public class DictionaryDeletedEvent : DomainEvent
{
    internal DictionaryDeletedEvent(long id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    public long Id { get; }

    /// <summary>
    /// Gets the code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; }
}
