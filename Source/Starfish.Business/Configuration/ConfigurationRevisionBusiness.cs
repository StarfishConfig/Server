using System.Text.Json;
using System.Text.Json.Serialization;
using Nerosoft.Euonia.Business;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// Business object for <see cref="ConfigurationRevision"/> aggregate.
/// </summary>
internal sealed class ConfigurationRevisionBusiness(IConfigurationRepository repository)
	: EditableObjectBase<ConfigurationRevisionBusiness, Configuration>
{
	private Configuration _aggregate;
	protected override Configuration Aggregate => _aggregate;

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

	private List<ConfigurationItem> Items { get; set; }

	#endregion

	#region Methods

	[FactoryFetch]
	private async Task FetchAsync(long id, CancellationToken cancellationToken = default)
	{
		_aggregate = await repository.GetAsync(id, false, [], cancellationToken);
		LoadProperty(IdProperty, _aggregate.Id);
		Items = await repository.GetItemsAsync(_aggregate.Id, cancellationToken);
	}

	[FactoryCreate]
	protected override Task InsertAsync(CancellationToken cancellationToken = default)
	{
		var revision = ConfigurationRevision.Create(Id);
		revision.SetVersion(Version);
		revision.SetComment(Comment);
		revision.SetOperator(Operator);
		revision.Data = JsonSerializer.Serialize(Items, new JsonSerializerOptions
		{
			WriteIndented = false,
			ReferenceHandler = ReferenceHandler.IgnoreCycles
		});
		return repository.InsertRevisionAsync(revision, cancellationToken);
	}

	#endregion
}