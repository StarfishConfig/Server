using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Event raised when a configuration is deleted.
/// </summary>
/// <param name="id"></param>
/// <param name="teamId"></param>
/// <param name="projectId"></param>
/// <param name="name"></param>
public class ConfigurationDeletedEvent(long id, long teamId, long projectId, string name)
	: DomainEvent
{
	/// <summary>
	/// Gets the identifier of the configuration that was deleted.
	/// </summary>
	public long Id { get; } = id;

	/// <summary>
	/// Gets the identifier of the team associated with the configuration that was deleted.
	/// </summary>
	public long TeamId { get; } = teamId;

	/// <summary>
	/// Gets the identifier of the project associated with the configuration that was deleted.
	/// </summary>
	public long ProjectId { get; } = projectId;

	/// <summary>
	/// Gets the name of the configuration that was deleted.
	/// </summary>
	public string Name { get; } = name;
}