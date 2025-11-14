using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public sealed class ConfigurationArchive : Aggregate<long>
{
	public string Data { get; set; }

	public string Operator { get; set; }

	public DateTime ArchiveTime { get; set; }

	internal static ConfigurationArchive Create(long configId)
	{
		var entity = new ConfigurationArchive()
		{
			Id = configId
		};

		return entity;
	}

	internal void Update(string data, string @operator)
	{
		Data = data;
		Operator = @operator;
		ArchiveTime = DateTime.Now;

		RaiseEvent(new ConfigurationArchiveUpdatedEvent());
	}
}