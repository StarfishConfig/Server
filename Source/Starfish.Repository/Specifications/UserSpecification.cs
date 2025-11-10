using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Specifications for querying User entities.
/// </summary>
internal static class UserSpecification
{
    /// <summary>
    /// Specification to check if User Id equals the given id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Specification<User> IdEquals(long id)
    {
        return new DirectSpecification<User>(t => t.Id == id);
    }

    /// <summary>
    /// Specification to check if User Id does not equal the given id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Specification<User> IdNotEquals(long id)
    {
        return new DirectSpecification<User>(t => t.Id != id);
    }

    /// <summary>
    /// Specification to check if Username equals the given username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public static Specification<User> UsernameEquals(string username)
    {
        username = username.Normalize(TextCaseType.Lower);
        return new DirectSpecification<User>(t => t.Username == username);
    }

    /// <summary>
    /// Specification to check if Username contains the given username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public static Specification<User> UsernameContains(string username)
    {
        username = username.Normalize(TextCaseType.Lower);
        return new DirectSpecification<User>(t => t.Username.Contains(username));
    }

    /// <summary>
    /// Specification to check if Nickname contains the given nickname.
    /// </summary>
    /// <param name="nickname"></param>
    /// <returns></returns>
    public static Specification<User> NickNameContains(string nickname)
    {
        nickname = nickname.Normalize(TextCaseType.Lower);
        return new DirectSpecification<User>(t => t.Nickname.ToLower().Contains(nickname));
    }

    /// <summary>
    /// Specification to check if Email equals the given email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static Specification<User> EmailEquals(string email)
    {
        email = email.Normalize(TextCaseType.Lower);
        return new DirectSpecification<User>(t => t.Email == email);
    }

    /// <summary>
    /// Specification to check if Email contains the given email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static Specification<User> EmailContains(string email)
    {
        email = email.Normalize(TextCaseType.Lower);
        return new DirectSpecification<User>(t => t.Email.Contains(email));
    }

    /// <summary>
    /// Specification to check if Phone equals the given phone.
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    public static Specification<User> PhoneEquals(string phone)
    {
        phone = phone.Normalize(TextCaseType.Lower);
        return new DirectSpecification<User>(t => t.Phone == phone);
    }

    /// <summary>
    /// Specification to check if Phone contains the given phone.
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    public static Specification<User> PhoneContains(string phone)
    {
        phone = phone.Normalize(TextCaseType.Lower);
        return new DirectSpecification<User>(t => t.Phone.Contains(phone));
    }

    /// <summary>
    /// Specification to check if any Authority matches the given provider and value.
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Specification<User> OpenIdEquals(string provider, string value)
    {
        provider = provider?.Trim().ToLowerInvariant() ?? throw new ArgumentNullException(nameof(provider));
        value = value.Normalize(TextCaseType.Lower);

        return new DirectSpecification<User>(x => x.Authorities.Any(t => t.Provider == provider && t.OpenId == value));
    }

    /// <summary>
    /// Specification to check if any of Username, Nickname, or Email contains the given keyword.
    /// </summary>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public static Specification<User> Matches(string keyword)
    {
        ISpecification<User>[] specifications =
        [
            UsernameContains(keyword),
            NickNameContains(keyword),
            EmailContains(keyword),
            PhoneContains(keyword)
        ];

        return new CompositeSpecification<User>(PredicateOperator.OrElse, specifications);
    }
}