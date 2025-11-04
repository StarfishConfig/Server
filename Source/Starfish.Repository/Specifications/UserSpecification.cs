using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Specifications for querying User entities.
/// </summary>
public static class UserSpecification
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
            EmailContains(keyword)
        ];

        return new CompositeSpecification<User>(PredicateOperator.OrElse, specifications);
    }
}