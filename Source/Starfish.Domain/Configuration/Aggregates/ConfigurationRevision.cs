using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public sealed class ConfigurationRevision : Entity<long>, IHasCreateTime
{
	private ConfigurationRevision()
	{
	}

	private ConfigurationRevision(long configurationId)
		: this()
	{
		ConfigurationId = configurationId;
	}

	private ConfigurationRevision(string version, string comment, string data, string @operator)
		: this()
	{
		Data = data;
		Comment = comment;
		Version = version;
		Operator = @operator;
	}

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

	internal static ConfigurationRevision Create(long configurationId)
	{
		return new ConfigurationRevision(configurationId);
	}
	
	internal static ConfigurationRevision Create(string version, string comment, string data, string @operator)
	{
		return new ConfigurationRevision(version, comment, data, @operator);
	}

	internal void SetVersion(string version)
	{
		Version = version;
	}

	internal void SetComment(string comment)
	{
		Comment = comment;
	}

	internal void SetOperator(string @operator)
	{
		Operator = @operator;
	}

	internal void SetData(string data)
	{
		Data = data;
	}
}