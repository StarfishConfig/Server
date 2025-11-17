using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public class ConfigurationSecretChangedEvent : EntityPropertyChangedEvent<long, string>
{
	public ConfigurationSecretChangedEvent(long id, string oldValue, string newValue)
		: base(id, oldValue, newValue)
	{
	}
}