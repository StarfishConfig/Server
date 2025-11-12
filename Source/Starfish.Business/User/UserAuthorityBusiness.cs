using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// The user authority business object.
/// </summary>
internal sealed class UserAuthorityBusiness : EditableObjectBase<UserAuthorityBusiness, User>, IDomainService
{
    private IUserRepository _repository;
    private IUserRepository Repository => _repository ??= LazyServiceProvider.GetService<IUserRepository>();

    private User _aggregate;
    protected override User Aggregate => _aggregate;

    public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(p => p.Id);
    public static readonly PropertyInfo<string> ProviderProperty = RegisterProperty<string>(p => p.Provider);
    public static readonly PropertyInfo<string> OpenIdProperty = RegisterProperty<string>(p => p.OpenId);
    public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);

    public long Id
    {
        get => GetProperty(IdProperty);
        private set => LoadProperty(IdProperty, value);
    }

    public string Provider
    {
        get => GetProperty(ProviderProperty);
        set => SetProperty(ProviderProperty, value);
    }

    public string OpenId
    {
        get => GetProperty(OpenIdProperty);
        set => SetProperty(OpenIdProperty, value);
    }

    public string Name
    {
        get => GetProperty(NameProperty);
        set => SetProperty(NameProperty, value);
    }

    [FactoryFetch]
    private async Task FetchAsync(long id, CancellationToken cancellationToken = default)
    {
        var aggregate = await Repository.GetAsync(id, true, [nameof(User.Authorities)], cancellationToken);

        _aggregate = aggregate ?? throw new NotFoundException();

        using (BypassRuleChecks)
        {
            Id = Aggregate.Id;
        }
    }

    [FactoryUpdate]
    protected override Task UpdateAsync(CancellationToken cancellationToken = default)
    {
        Aggregate.CreateAuthority(Provider, OpenId, Name);
        return Repository.UpdateAsync(Aggregate, true, cancellationToken);
    }

    [FactoryDelete]
    protected override Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        Aggregate.RemoveAuthority(Provider, OpenId);
        return Repository.UpdateAsync(Aggregate, true, cancellationToken);
    }
}