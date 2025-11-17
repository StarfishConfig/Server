namespace Nerosoft.Starfish.Domain;

public class ConfigurationNameChangedEvent : EntityPropertyChangedEvent<long, string>
{
	public ConfigurationNameChangedEvent(long id, string oldValue, string newValue)
		: base(id, oldValue, newValue)
	{
	}
}