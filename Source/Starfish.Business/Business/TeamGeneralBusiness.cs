using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

internal partial class TeamGeneralBusiness : EditableObjectBase<TeamGeneralBusiness, Team>, IDomainService
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

    [FactoryFetch]
    protected async Task FetchAsync(long id, CancellationToken cancellationToken = default)
    {
        var aggregate = await Repository.GetAsync(id, true, cancellationToken);
        _aggregate = aggregate ?? throw new NotFoundException();
        using (BypassRuleChecks)
        {
            Id = aggregate.Id;
            Name = aggregate.Name;
            Description = aggregate.Description;
        }
    }

    [FactoryInsert]
    protected override Task InsertAsync(CancellationToken cancellationToken = default)
    {
        _aggregate = Team.Create(Name, Identity.GetUserIdOfInt64());
        if (!string.IsNullOrWhiteSpace(Description))
        {
            _aggregate.SetDescription(Description);
        }

        return Repository.InsertAsync(Aggregate, true, cancellationToken)
                         .ContinueWith(task =>
                         {
                             task.WaitAndUnwrapException(cancellationToken);
                             Id = task.Result.Id;
                         }, cancellationToken);
    }

    [FactoryUpdate]
    protected override Task UpdateAsync(CancellationToken cancellationToken = default)
    {
        if (!HasChangedProperties)
        {
            return Task.CompletedTask;
        }

        if (Aggregate.OwnerId != Identity.GetUserIdOfInt64())
        {
            throw new ForbiddenException();
        }

        if (ChangedProperties.Contains(NameProperty))
        {
            Aggregate.SetName(Name);
        }

        if (ChangedProperties.Contains(DescriptionProperty))
        {
            Aggregate.SetDescription(Description);
        }

        return Repository.UpdateAsync(Aggregate, true, cancellationToken);
    }

    [FactoryDelete]
    protected override Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        if (Aggregate.OwnerId != Identity.GetUserIdOfInt64())
        {
            throw new ForbiddenException();
        }

        return Repository.DeleteAsync(Aggregate, () => new TeamDeletedEvent(Aggregate.Id, Aggregate.Name), true, cancellationToken);
    }
}