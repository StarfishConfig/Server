using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Repository for managing User entities.
/// </summary>
internal class UserRepository : BaseRepository<IdentityDataContext, User, long>, IUserRepository
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

    public Task<User> FindByProviderAsync(string provider, string value, bool tracking, CancellationToken cancellationToken = default)
    {
        provider = provider?.Trim().ToLowerInvariant();

        ISpecification<User> specification;

        switch (provider)
        {
            case null or "":
                throw new ArgumentNullException(nameof(provider));
            case AuthenticationConstant.Provider.Email:
                specification = UserSpecification.EmailEquals(value);
                break;
            case AuthenticationConstant.Provider.Phone:
                specification = UserSpecification.PhoneEquals(value);
                break;
            case AuthenticationConstant.Provider.Username:
                specification = UserSpecification.UsernameEquals(value);
                break;
            case AuthenticationConstant.Provider.Identifier:
                specification = UserSpecification.IdEquals(long.Parse(value));
                break;
            case AuthenticationConstant.Provider.Apple:
            case AuthenticationConstant.Provider.Wechat:
            case AuthenticationConstant.Provider.Weibo:
            case AuthenticationConstant.Provider.LinkedIn:
            case AuthenticationConstant.Provider.Twitter:
            case AuthenticationConstant.Provider.Facebook:
            case AuthenticationConstant.Provider.Google:
            case AuthenticationConstant.Provider.Github:
            case AuthenticationConstant.Provider.Microsoft:
            case AuthenticationConstant.Provider.QQ:
                specification = UserSpecification.OpenIdEquals(provider, value);
                break;
            default:
                throw new NotSupportedException($"The provider '{provider}' is not supported.");
        }

        var predicate = specification.Satisfy();
        return GetAsync(predicate, tracking, [nameof(User.Authorities)], cancellationToken);
    }
}