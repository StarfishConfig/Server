using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Defines the configuration archive aggregate.
/// </summary>
public sealed class ConfigurationArchive : Aggregate<long>
{
	/// <summary>
	/// Gets or sets the version of the configuration.
	/// </summary>
	public string Version { get; set; }

	/// <summary>
	/// Gets or sets the data of the configuration.
	/// </summary>
	public string Data { get; set; }

	/// <summary>
	/// Gets or sets the operator who made the archive.
	/// </summary>
	public string Operator { get; set; }

	/// <summary>
	/// Gets or sets the time when the configuration was archived.
	/// </summary>
	public DateTime ArchiveTime { get; set; }

	internal static ConfigurationArchive Create(long configId)
	{
		var entity = new ConfigurationArchive()
		{
			Id = configId
		};

		return entity;
	}

	internal void Update(string version, string data, string @operator)
	{
		Version = version;
		Data = data;
		Operator = @operator;
		ArchiveTime = DateTime.Now;

		RaiseEvent(new ConfigurationArchiveUpdatedEvent());
	}
}