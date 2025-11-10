using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Repository for managing User entities.
/// </summary>
internal class UserRepository : BaseRepository<AccountDataContext, User, long>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="provider"></param>
    public UserRepository(IContextProvider provider)
        : base(provider)
    {
    }

    /// <inheritdoc />
    public Task<User> FindByUsernameAsync(string username, bool tracking, string[] properties, CancellationToken cancellationToken = default)
    {
        return GetAsync(t => t.Username == username, tracking, properties, cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> CheckUsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        var specification = UserSpecification.UsernameEquals(username);
        var predicate = specification.Satisfy();
        return AnyAsync(predicate, null, cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> CheckEmailExistsAsync(string email, long ignoreId, CancellationToken cancellationToken = default)
    {
        ISpecification<User>[] specifications =
        [
            UserSpecification.EmailEquals(email),
            UserSpecification.IdNotEquals(ignoreId)
        ];
        var predicate = new CompositeSpecification<User>(PredicateOperator.AndAlso, specifications).Satisfy();
        return AnyAsync(predicate, null, cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> CheckPhoneExistsAsync(string phone, long ignoreId, CancellationToken cancellationToken = default)
    {
        ISpecification<User>[] specifications =
        [
            UserSpecification.EmailEquals(phone),
            UserSpecification.IdNotEquals(ignoreId)
        ];
        var predicate = new CompositeSpecification<User>(PredicateOperator.AndAlso, specifications).Satisfy();
        return AnyAsync(predicate, null, cancellationToken);
    }
}