using Nerosoft.Euonia.Business;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// Business logic for Project General information.
/// </summary>
[SuppressMessage("Style", "IDE0051")]
internal class ProjectGeneralBusiness : EditableObjectBase<ProjectGeneralBusiness, Project>
{
	private IProjectRepository Repository => LazyServiceProvider.GetRequiredService<IProjectRepository>();

	private Project _aggregate;
	protected override Project Aggregate => _aggregate;

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

	public static readonly PropertyInfo<string> UrlProperty = RegisterProperty<string>(p => p.Url);

	public string Url
	{
		get => GetProperty(UrlProperty);
		set => SetProperty(UrlProperty, value);
	}

	public static readonly PropertyInfo<string> ImageProperty = RegisterProperty<string>(p => p.Image);

	public string Image
	{
		get => GetProperty(ImageProperty);
		set => SetProperty(ImageProperty, value);
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
		_aggregate = await Repository.GetAsync(id, true, [], cancellationToken);
		if (_aggregate == null)
		{
			throw new NotFoundException($"Project with Id {id} not found.");
		}

		using (BypassRuleChecks)
		{
			LoadProperty(TeamIdProperty, _aggregate.TeamId);
			LoadProperty(NameProperty, _aggregate.Name);
			LoadProperty(DescriptionProperty, _aggregate.Description);
			LoadProperty(UrlProperty, _aggregate.Url);
			LoadProperty(ImageProperty, _aggregate.Image);
		}
	}

	[FactoryInsert]
	protected override async Task InsertAsync(CancellationToken cancellationToken = default)
	{
		_aggregate = Project.Create(TeamId, Name);
		Aggregate.SetDescription(Description);
		Aggregate.SetUrl(Url);
		Aggregate.SetImage(Image);
		await Repository.InsertAsync(Aggregate, true, cancellationToken);

		LoadProperty(IdProperty, _aggregate.Id);
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
		if (ChangedProperties.Contains(UrlProperty))
		{
			Aggregate.SetUrl(Url);
		}
		if (ChangedProperties.Contains(ImageProperty))
		{
			Aggregate.SetImage(Image);
		}

		await Repository.UpdateAsync(Aggregate, true, cancellationToken);
	}

	[FactoryDelete]
	protected override Task DeleteAsync(CancellationToken cancellationToken = default)
	{
		if (Aggregate == null)
		{
			throw new InvalidOperationException("Aggregate is null. Cannot delete.");
		}
		return Repository.DeleteAsync(Aggregate, () => new ProjectDeletedEvent(Id, TeamId, Name), true, cancellationToken);
	}
	#endregion
}
