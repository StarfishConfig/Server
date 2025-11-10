using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Specifications for querying Token entities.
/// </summary>
internal static class TokenSpecification
{
    /// <summary>
    /// Specification to check if Token Key equals the given key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Specification<Token> KeyEquals(string key)
    {
        return new DirectSpecification<Token>(t => t.Key == key);
    }

    /// <summary>
    /// Specification to check if Token Subject equals the given subject.
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public static Specification<Token> SubjectEquals(long subject)
    {
        return new DirectSpecification<Token>(t => t.Subject == subject);
    }

    /// <summary>
    /// Specification to check if Token Type equals the given type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Specification<Token> TypeEquals(string type)
    {
        return new DirectSpecification<Token>(t => t.Type == type);
    }

    /// <summary>
    /// Specification to check if Token expires before the given dateTime.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static Specification<Token> ExpiresBefore(DateTime dateTime)
    {
        return new DirectSpecification<Token>(t => t.Expires != null && t.Expires < dateTime);
    }
}