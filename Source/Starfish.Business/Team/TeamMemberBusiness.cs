using Nerosoft.Euonia.Business;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// Business logic for team members.
/// </summary>
internal class TeamMemberBusiness : EditableObjectBase<TeamMemberBusiness, Team>
{
    private ITeamRepository Repository => LazyServiceProvider.GetRequiredService<ITeamRepository>();

    private Team _aggregate;
    protected override Team Aggregate => _aggregate;

    #region Properties

    public static readonly PropertyInfo<List<long>> UserIdsProperty = RegisterProperty<List<long>>(p => p.UserIds);

    public List<long> UserIds
    {
        get => GetProperty(UserIdsProperty);
        set => SetProperty(UserIdsProperty, value);
    }

    public static readonly PropertyInfo<string> ReasonProperty = RegisterProperty<string>(p => p.Reason);

    public string Reason
    {
        get => GetProperty(ReasonProperty);
        set => SetProperty(ReasonProperty, value);
    }

    #endregion

    [FactoryFetch]
    private async Task FetchAsync(long id, CancellationToken cancellationToken = default)
    {
        _aggregate = await Repository.GetAsync(id, true, [nameof(Team.Members)], cancellationToken);
    }

    [FactoryUpdate]
    protected override async Task UpdateAsync(CancellationToken cancellationToken = default)
    {
    }

    [FactoryDelete]
    protected override Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(cancellationToken);
    }
}