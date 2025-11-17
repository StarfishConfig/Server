using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public class ConfigurationDisabledEvent(long id) : DomainEvent
{
	/// <summary>
	/// Gets the identifier of the entity whose property has changed.
	/// </summary>
	public long Id { get; } = id;
}