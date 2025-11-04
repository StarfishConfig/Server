using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Specifications for querying Team entities.
/// </summary>
public static class TeamSpecification
{
    /// <summary>
    /// Specification to check if Team Id equals the given id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Specification<Team> IdEquals(long id)
    {
        return new DirectSpecification<Team>(t => t.Id == id);
    }

    /// <summary>
    /// Specification to check if Team Name equals the given name (case insensitive).
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Specification<Team> NameEquals(string name)
    {
        name = name.Normalize(TextCaseType.Lower);
        return new DirectSpecification<Team>(t => t.Name.ToLower() == name);
    }

    /// <summary>
    /// Specification to check if Team Name contains the given substring (case insensitive).
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Specification<Team> NameContains(string name)
    {
        name = name.Normalize(TextCaseType.Lower);
        return new DirectSpecification<Team>(t => t.Name.ToLower().Contains(name));
    }

    /// <summary>
    /// Specification to check if Team Description contains the given substring (case insensitive).
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    public static Specification<Team> DescriptionContains(string description)
    {
        description = description.Normalize(TextCaseType.Lower);
        return new DirectSpecification<Team>(t => t.Description.ToLower().Contains(description));
    }

    /// <summary>
    /// Specification to check if Team matches the given keyword in Name or Description.
    /// </summary>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public static Specification<Team> Matches(string keyword)
    {
        ISpecification<Team>[] specifications =
        [
            NameContains(keyword),
            DescriptionContains(keyword)
        ];

        return new CompositeSpecification<Team>(PredicateOperator.OrElse, specifications);
    }

    /// <summary>
    /// Specification to check if Team has a member with the given userId.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static Specification<Team> HasMember(long userId)
    {
        return new DirectSpecification<Team>(t => t.Members.Any(m => m.UserId == userId));
    }
}