using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// The user password business object.
/// </summary>
internal class UserPasswordBusiness : EditableObjectBase<UserPasswordBusiness, User>, IDomainService
{
    private IUserRepository _repository;
    private IUserRepository Repository => _repository ??= LazyServiceProvider.GetService<IUserRepository>();

    private User _aggregate;
    protected override User Aggregate => _aggregate;

    public static readonly PropertyInfo<string> PasswordProperty = RegisterProperty<string>(p => p.Password);
    public static readonly PropertyInfo<string> ChangeTypeProperty = RegisterProperty<string>(p => p.ChangeType);

    /// <summary>
    /// Get or set the password.
    /// </summary>
    public string Password
    {
        get => GetProperty(PasswordProperty);
        set => SetProperty(PasswordProperty, value);
    }

    public string ChangeType
    {
        get => GetProperty(ChangeTypeProperty);
        set => SetProperty(ChangeTypeProperty, value);
    }

    [FactoryFetch]
    protected async Task FetchAsync(long id, CancellationToken cancellationToken = default)
    {
        var aggregate = await Repository.GetAsync(id, true, cancellationToken);

        _aggregate = aggregate ?? throw new NotFoundException();
    }

    [FactoryUpdate]
    protected override Task UpdateAsync(CancellationToken cancellationToken = default)
    {
        if (!HasChangedProperties)
        {
            return Task.CompletedTask;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            throw new BadRequestException("Password cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(ChangeType))
        {
            throw new BadRequestException("Change type cannot be empty.");
        }

        if (string.Equals(ChangeType, "update") && Aggregate.Id != Identity.GetUserIdOfInt64())
        {
            throw new ForbiddenException();
        }

        Aggregate.SetPassword(Password, ChangeType);
        return Repository.UpdateAsync(Aggregate, true, cancellationToken);
    }
}