using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public sealed class ConfigurationRevision : Entity<long>, IHasCreateTime
{
	/// <summary>
	/// Gets or sets the identifier of the configuration this revision belongs to.
	/// </summary>
	public long ConfigurationId { get; set; }

	public string Data { get; set; }

	public string Comment { get; set; }

	public string Version { get; set; }

	public string Operator { get; set; }

	/// <summary>
	/// Gets or sets the identifier of the configuration this revision belongs to.
	/// </summary>
	public DateTime CreateTime { get; set; }

	public Configuration Configuration { get; set; }
}