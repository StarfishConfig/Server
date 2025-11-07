using Nerosoft.Euonia.Business;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// The user general business object.
/// </summary>
/// <param name="provider"></param>
internal partial class UserGeneralBusiness : EditableObjectBase<UserGeneralBusiness, User>
{
    private IUserRepository _repository;
    private IUserRepository Repository => _repository ??= LazyServiceProvider.GetService<IUserRepository>();

    private User _aggregate;
    protected override User Aggregate => _aggregate;

    public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(p => p.Id);
    public static readonly PropertyInfo<string> UsernameProperty = RegisterProperty<string>(p => p.Username);
    public static readonly PropertyInfo<string> PasswordProperty = RegisterProperty<string>(p => p.Password);
    public static readonly PropertyInfo<string> NicknameProperty = RegisterProperty<string>(p => p.Nickname);
    public static readonly PropertyInfo<string> EmailProperty = RegisterProperty<string>(p => p.Email);
    public static readonly PropertyInfo<string> PhoneProperty = RegisterProperty<string>(p => p.Phone);
    public static readonly PropertyInfo<int> SourceProperty = RegisterProperty<int>(p => p.Source);

    /// <summary>
    /// Get the identifier.
    /// </summary>
    public long Id
    {
        get => GetProperty(IdProperty);
        private set => LoadProperty(IdProperty, value);
    }

    /// <inheritdoc cref="User.Username"/>
    public string Username
    {
        get => GetProperty(UsernameProperty);
        set => SetProperty(UsernameProperty, value);
    }

    /// <summary>
    /// Get or set the password.
    /// </summary>
    public string Password
    {
        get => GetProperty(PasswordProperty);
        set => SetProperty(PasswordProperty, value);
    }

    /// <inheritdoc cref="User.Nickname"/>
    public string Nickname
    {
        get => GetProperty(NicknameProperty);
        set => SetProperty(NicknameProperty, value);
    }

    /// <inheritdoc cref="User.Email"/>
    public string Email
    {
        get => GetProperty(EmailProperty);
        set => SetProperty(EmailProperty, value);
    }

    /// <inheritdoc cref="User.Phone"/>
    public string Phone
    {
        get => GetProperty(PhoneProperty);
        set => SetProperty(PhoneProperty, value);
    }

    /// <inheritdoc cref="User.Source"/>
    public int Source
    {
        get => GetProperty(SourceProperty);
        set => SetProperty(SourceProperty, value);
    }

    protected override void AddRules()
    {
        Rules.AddRule(new UsernameAvailabilityCheckRule());
        Rules.AddRule(new EmailAvailabilityCheckRule());
        Rules.AddRule(new PhoneAvailabilityCheckRule());
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
            Id = Aggregate.Id;
            Username = Aggregate.Username;
            Nickname = Aggregate.Nickname;
            Email = Aggregate.Email;
            Phone = Aggregate.Phone;
        }
    }

    [FactoryInsert]
    protected override Task InsertAsync(CancellationToken cancellationToken = default)
    {
        var user = User.Create(Username, Source);
        if (!string.IsNullOrWhiteSpace(Email))
        {
            user.SetEmail(Email);
        }

        if (!string.IsNullOrWhiteSpace(Phone))
        {
            user.SetPhone(Phone);
        }

        if (!string.IsNullOrWhiteSpace(Password))
        {
            user.SetPassword(Password);
        }

        user.SetNickname(Nickname ?? Username);

        return Repository.InsertAsync(user, true, cancellationToken)
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

        if (ChangedProperties.Contains(EmailProperty))
        {
            Aggregate.SetEmail(Email);
        }

        if (ChangedProperties.Contains(PhoneProperty))
        {
            Aggregate.SetPhone(Phone);
        }

        if (ChangedProperties.Contains(NicknameProperty))
        {
            Aggregate.SetNickname(Nickname);
        }

        return Repository.UpdateAsync(Aggregate, true, cancellationToken);
    }
}
