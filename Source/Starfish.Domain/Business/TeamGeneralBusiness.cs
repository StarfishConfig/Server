using Nerosoft.Euonia.Business;

namespace Nerosoft.Starfish.Domain;

internal partial class TeamGeneralBusiness : EditableObjectBase<TeamGeneralBusiness, Team>
{
    private ITeamRepository _repository;
    private ITeamRepository Repository => _repository ??= LazyServiceProvider.GetService<ITeamRepository>();

    private Team _aggregate;
    protected override Team Aggregate => _aggregate;

    public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(p => p.Id);
    public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
    public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(p => p.Description);

    /// <summary>
    /// Get the identifier.
    /// </summary>
    public long Id
    {
        get => GetProperty(IdProperty);
        private set => LoadProperty(IdProperty, value);
    }

    /// <inheritdoc cref="Team.Name"/>
    public string Name
    {
        get => GetProperty(NameProperty);
        set => SetProperty(NameProperty, value);
    }

    /// <inheritdoc cref="Team.Description"/>
    public string Description
    {
        get => GetProperty(DescriptionProperty);
        set => SetProperty(DescriptionProperty, value);
    }

    [FactoryCreate]
    protected override Task CreateAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }


}
