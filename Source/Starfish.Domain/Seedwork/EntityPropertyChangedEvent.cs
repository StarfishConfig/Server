using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines a domain event for when an entity's property has changed.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TProperty"></typeparam>
public abstract class EntityPropertyChangedEvent<TKey, TProperty> : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityPropertyChangedEvent{TKey, TProperty}"/> class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected EntityPropertyChangedEvent(TKey id, TProperty oldValue, TProperty newValue)
    {
        Id = id;
        OldValue = oldValue;
        NewValue = newValue;
    }

    /// <summary>
    /// Gets the identifier of the entity whose property has changed.
    /// </summary>
    public TKey Id { get; }

    /// <summary>
    /// Gets the old value of the property before the change.
    /// </summary>
    public TProperty OldValue { get; }

    /// <summary>
    /// Gets the new value of the property after the change.
    /// </summary>
    public TProperty NewValue { get; }
}