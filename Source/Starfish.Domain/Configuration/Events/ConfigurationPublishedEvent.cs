using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Event raised when a configuration is published.
/// </summary>
/// <param name="id"></param>
/// <param name="version"></param>
/// <param name="comment"></param>
/// <param name="operator"></param>
public class ConfigurationPublishedEvent(long id, string version, string comment, string @operator) : DomainEvent
{
	/// <summary>
	/// Gets the identifier of the entity whose property has changed.
	/// </summary>
	public long Id { get; } = id;

	/// <summary>
	/// Gets the version of the configuration that was published.
	/// </summary>
	public string Version { get; } = version;

	/// <summary>
	/// Gets the comment associated with the publishing action.
	/// </summary>
	public string Comment { get; } = comment;

	/// <summary>
	/// Gets or sets the operator who performed the publishing action.
	/// </summary>
	public string Operator { get; } = @operator;
}