using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

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
}