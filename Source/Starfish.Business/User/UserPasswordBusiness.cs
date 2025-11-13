using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// The user password business object.
/// </summary>
internal class UserPasswordBusiness : EditableObjectBase<UserPasswordBusiness, User>
{
    private IUserRepository Repository => LazyServiceProvider.GetService<IUserRepository>();

    private User _aggregate;
    protected override User Aggregate => _aggregate;

    public static readonly PropertyInfo<string> PasswordProperty = RegisterProperty<string>(p => p.Password);
    public static readonly PropertyInfo<string> ActionTypeProperty = RegisterProperty<string>(p => p.ActionType);

    /// <summary>
    /// Get or set the password.
    /// </summary>
    public string Password
    {
        get => GetProperty(PasswordProperty);
        set => SetProperty(PasswordProperty, value);
    }

    public string ActionType
    {
        get => GetProperty(ActionTypeProperty);
        set => SetProperty(ActionTypeProperty, value);
    }

    /// <summary>
    /// Fetch the user by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NotFoundException"></exception>
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

        if (string.IsNullOrWhiteSpace(ActionType))
        {
            throw new BadRequestException("Change type cannot be empty.");
        }

        if (string.Equals(ActionType, UserPasswordChangeTypeConstant.Change) && Aggregate.Id != Identity.GetUserIdOfInt64())
        {
            throw new ForbiddenException();
        }

        Aggregate.SetPassword(Password, ActionType);
        return Repository.UpdateAsync(Aggregate, true, cancellationToken);
    }
}