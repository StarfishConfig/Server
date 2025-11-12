using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// The user lockout business object.
/// </summary>
internal sealed class UserLockoutBusiness : CommandObjectBase<UserLockoutBusiness>, IDomainService
{
    private IUserRepository _repository;
    private IUserRepository Repository => _repository ??= LazyServiceProvider.GetRequiredService<IUserRepository>();

    /// <summary>
    /// Execute the user lockout operation.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [FactoryExecute]
    public async Task ExecuteAsync(long id, string type, CancellationToken cancellationToken = default)
    {
        var user = await Repository.GetAsync(id, true, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        switch (type)
        {
            case "increase":
                user.IncrementAccessFailedCount();
                break;
            case "reset":
                user.ResetAccessFailedCount();
                break;
        }

        await Repository.UpdateAsync(user, true, cancellationToken);
    }

    /// <summary>
    /// Execute the user lockout operation by username.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="type"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NotFoundException"></exception>
    [FactoryExecute]
    public async Task ExecuteAsync(string username, string type, CancellationToken cancellationToken = default)
    {
        var user = await Repository.FindByUsernameAsync(username, true, [], cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        switch (type)
        {
            case "increase":
                user.IncrementAccessFailedCount();
                break;
            case "reset":
                user.ResetAccessFailedCount();
                break;
        }

        await Repository.UpdateAsync(user, true, cancellationToken);
    }
}