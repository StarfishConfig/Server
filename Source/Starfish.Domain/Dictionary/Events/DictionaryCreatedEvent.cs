using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines an event that is triggered when a <see cref="DictionaryRoot"/> is created.
/// </summary>
public class DictionaryCreatedEvent : DomainEvent
{
    /// <summary>
    /// Constructor with code and name.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="name"></param>
    internal DictionaryCreatedEvent(string code, string name)
    {
        Code = code;
        Name = name;
    }

    /// <summary>
    /// Gets the code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; }
}
