using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Event raised when a new configuration is created.
/// </summary>
public sealed class ConfigurationCreatedEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ConfigurationCreatedEvent"/> class.
	/// </summary>
	/// <param name="teamId"></param>
	/// <param name="projectId"></param>
	/// <param name="name"></param>
	public ConfigurationCreatedEvent(long teamId, long projectId, string name)
	{
		TeamId = teamId;
		ProjectId = projectId;
		Name = name;
	}

	public long TeamId { get; }

	public long ProjectId { get; }

	public string Name { get; }
}