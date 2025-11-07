using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// The user password business object.
/// </summary>
internal class UserPasswordBusiness : CommandObjectBase<UserPasswordBusiness>, IDomainService
{
    private IUserRepository _repository;
    private IUserRepository Repository => _repository ??= LazyServiceProvider.GetService<IUserRepository>();

    [FactoryExecute]
    protected async Task ExecuteAsync(long userId, string password, string changeType, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new BadRequestException("Password cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(changeType))
        {
            throw new BadRequestException("Change type cannot be empty.");
        }

        var user = await Repository.GetAsync(userId, true, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException();
        }

        if (string.Equals(changeType, "update") && user.Id != Identity.GetUserIdOfInt64())
        {
            throw new ForbiddenException();
        }

        user.SetPassword(password, changeType);
        await Repository.UpdateAsync(user, true, cancellationToken);
    }
}
