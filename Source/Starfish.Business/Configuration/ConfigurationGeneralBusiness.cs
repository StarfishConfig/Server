using Nerosoft.Euonia.Business;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// Business object for <see cref="Configuration"/> aggregate.
/// </summary>
internal sealed class ConfigurationGeneralBusiness(IConfigurationRepository repository)
	: EditableObjectBase<ConfigurationGeneralBusiness, Configuration>
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

	public static readonly PropertyInfo<long> TeamIdProperty = RegisterProperty<long>(p => p.TeamId);

	public long TeamId
	{
		get => GetProperty(TeamIdProperty);
		set => SetProperty(TeamIdProperty, value);
	}

	public static readonly PropertyInfo<long> ProjectIdProperty = RegisterProperty<long>(p => p.ProjectId);

	public long ProjectId
	{
		get => GetProperty(ProjectIdProperty);
		set => SetProperty(ProjectIdProperty, value);
	}

	public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);

	public string Name
	{
		get => GetProperty(NameProperty);
		set => SetProperty(NameProperty, value);
	}

	public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(p => p.Description);

	public string Description
	{
		get => GetProperty(DescriptionProperty);
		set => SetProperty(DescriptionProperty, value);
	}

	public static readonly PropertyInfo<string> SecretProperty = RegisterProperty<string>(p => p.Secret);

	public string Secret
	{
		get => GetProperty(SecretProperty);
		set => SetProperty(SecretProperty, value);
	}

	#endregion

	#region Methods

	[FactoryCreate]
	protected override Task CreateAsync(CancellationToken cancellationToken = default)
	{
		return Task.CompletedTask;
	}

	[FactoryFetch]
	private async Task FetchAsync(long id, CancellationToken cancellationToken = default)
	{
		_aggregate = await repository.GetAsync(id, true, [], cancellationToken);

		LoadProperty(TeamIdProperty, _aggregate.TeamId);
		LoadProperty(ProjectIdProperty, _aggregate.ProjectId);
		LoadProperty(NameProperty, _aggregate.Name);
		LoadProperty(DescriptionProperty, _aggregate.Description);
	}

	[FactoryInsert]
	protected override async Task InsertAsync(CancellationToken cancellationToken = default)
	{
		_aggregate = Configuration.Create(TeamId, ProjectId, Name);

		Aggregate.SetDescription(Description);

		await repository.InsertAsync(_aggregate, true, cancellationToken);
		Id = Aggregate.Id;
	}

	[FactoryUpdate]
	protected override async Task UpdateAsync(CancellationToken cancellationToken = default)
	{
		if (!HasChangedProperties)
		{
			return;
		}

		if (ChangedProperties.Contains(NameProperty))
		{
			Aggregate.SetName(Name);
		}

		if (ChangedProperties.Contains(DescriptionProperty))
		{
			Aggregate.SetDescription(Description);
		}

		await repository.UpdateAsync(_aggregate, true, cancellationToken);
	}

	[FactoryDelete]
	protected override async Task DeleteAsync(CancellationToken cancellationToken = default)
	{
		await repository.DeleteAsync(Aggregate, () => new ConfigurationDeletedEvent(Aggregate.Id, Aggregate.TeamId, Aggregate.ProjectId, Aggregate.Name), true, cancellationToken);
	}

	#endregion
}