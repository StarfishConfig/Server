using System.Text.Json;
using System.Text.Json.Serialization;
using Nerosoft.Euonia.Business;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

internal sealed class ConfigurationArchiveBusiness(IConfigurationRepository repository)
	: EditableObjectBase<ConfigurationArchiveBusiness, ConfigurationArchive>
{
	private ConfigurationArchive _aggregate;
	protected override ConfigurationArchive Aggregate => _aggregate;

	private List<ConfigurationItem> Items { get; set; }

	#region Properties

	public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(p => p.Id);

	public long Id
	{
		get => GetProperty(IdProperty);
		set => LoadProperty(IdProperty, value);
	}


	public static readonly PropertyInfo<string> VersionProperty = RegisterProperty<string>(p => p.Version);

	public string Version
	{
		get => GetProperty(VersionProperty);
		set => SetProperty(VersionProperty, value);
	}

	public static readonly PropertyInfo<string> CommentProperty = RegisterProperty<string>(p => p.Comment);

	public string Comment
	{
		get => GetProperty(CommentProperty);
		set => SetProperty(CommentProperty, value);
	}

	public static readonly PropertyInfo<string> OperatorProperty = RegisterProperty<string>(p => p.Operator);

	public string Operator
	{
		get => GetProperty(OperatorProperty);
		set => SetProperty(OperatorProperty, value);
	}

	#endregion

	#region Methods

	[FactoryFetch]
	private async Task FetchAsync(long id, CancellationToken cancellationToken = default)
	{
		_aggregate = await repository.GetArchiveAsync(id, false, cancellationToken: cancellationToken);
		LoadProperty(IdProperty, _aggregate.Id);


		if (_aggregate == null)
		{
			MarkAsUpdate();
		}
		else
		{
			_aggregate = ConfigurationArchive.Create(Id);
			MarkAsInsert();
		}
	}

	[FactoryInsert]
	protected override async Task InsertAsync(CancellationToken cancellationToken = default)
	{
		Items = await repository.GetItemsAsync(_aggregate.Id, cancellationToken);
		var data = Items.ToDictionary(t => t.Key, t => t.Value);
		var jsonOptions = new JsonSerializerOptions
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
		};
		var serializedData = JsonSerializer.Serialize(data, jsonOptions);
		Aggregate.Update(Version, serializedData, Operator);
		await repository.InsertArchiveAsync(Aggregate, cancellationToken);
	}

	[FactoryUpdate]
	protected override async Task UpdateAsync(CancellationToken cancellationToken = default)
	{
		Items = await repository.GetItemsAsync(_aggregate.Id, cancellationToken);
		var data = Items.ToDictionary(t => t.Key, t => t.Value);
		var jsonOptions = new JsonSerializerOptions
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
		};
		var serializedData = JsonSerializer.Serialize(data, jsonOptions);
		Aggregate.Update(Version, serializedData, Operator);
		await repository.UpdateArchiveAsync(Aggregate, cancellationToken);
	}

	#endregion
}