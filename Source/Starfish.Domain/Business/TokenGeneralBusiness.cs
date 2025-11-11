using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// Business object for general token operations.
/// </summary>
internal sealed class TokenGeneralBusiness : EditableObjectBase<TokenGeneralBusiness>, IDomainService
{
    private ITokenRepository _repository;
    private ITokenRepository Repository => _repository ??= LazyServiceProvider.GetRequiredService<ITokenRepository>();

    #region Properties

    public static readonly PropertyInfo<string> TypeProperty = RegisterProperty<string>(p => p.Type);

    public string Type
    {
        get => GetProperty(TypeProperty);
        set => SetProperty(TypeProperty, value);
    }

    public static readonly PropertyInfo<long> SubjectProperty = RegisterProperty<long>(p => p.Subject);

    public long Subject
    {
        get => GetProperty(SubjectProperty);
        set => SetProperty(SubjectProperty, value);
    }

    public static readonly PropertyInfo<string> AccessTokenProperty = RegisterProperty<string>(p => p.AccessToken);

    public string AccessToken
    {
        get => GetProperty(AccessTokenProperty);
        set => SetProperty(AccessTokenProperty, value);
    }

    public static readonly PropertyInfo<DateTime> IssuedProperty = RegisterProperty<DateTime>(p => p.Issued);

    public DateTime Issued
    {
        get => GetProperty(IssuedProperty);
        set => SetProperty(IssuedProperty, value);
    }

    public static readonly PropertyInfo<DateTime?> ExpiresProperty = RegisterProperty<DateTime?>(p => p.Expires);

    public DateTime? Expires
    {
        get => GetProperty(ExpiresProperty);
        set => SetProperty(ExpiresProperty, value);
    }

    #endregion

    #region Methods

    [FactoryCreate]
    protected override Task CreateAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    [FactoryInsert]
    protected override Task InsertAsync(CancellationToken cancellationToken = default)
    {
        var aggregate = Token.Create(Type, Subject, AccessToken, Issued, Expires);
        return Repository.InsertAsync(aggregate, true, cancellationToken);
    }

    #endregion
}