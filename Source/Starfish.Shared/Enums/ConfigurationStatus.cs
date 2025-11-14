namespace Nerosoft.Starfish.Shared;

/// <summary>
/// Defines the status of a configuration.
/// </summary>
public enum ConfigurationStatus
{
	/// <summary>
	/// No status assigned.
	/// </summary>
	None = 0,

	/// <summary>
	/// Configuration is pending.
	/// </summary>
	Pending = 1,

	/// <summary>
	/// Configuration is published.
	/// </summary>
	Published = 2,

	/// <summary>
	/// Configuration is disabled.
	/// </summary>
	Disabled = 3
}