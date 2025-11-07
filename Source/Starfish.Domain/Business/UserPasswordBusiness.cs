using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// The user password business object.
/// </summary>
internal class UserPasswordBusiness : CommandObjectBase<UserPasswordBusiness>, IDomainService
{
    private IUserRepository _repository;
    private IUserRepository Repository => _repository ??= LazyServiceProvider.GetService<IUserRepository>();


}
