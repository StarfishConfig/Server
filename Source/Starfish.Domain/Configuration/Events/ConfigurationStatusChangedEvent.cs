using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Event that is raised when the status of a configuration changes.
/// </summary>
public class ConfigurationStatusChangedEvent : EntityPropertyChangedEvent<long, ConfigurationStatus>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ConfigurationStatusChangedEvent"/> class.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="oldValue"></param>
	/// <param name="newValue"></param>
	public ConfigurationStatusChangedEvent(long id, ConfigurationStatus oldValue, ConfigurationStatus newValue)
		: base(id, oldValue, newValue)
	{
	}
}