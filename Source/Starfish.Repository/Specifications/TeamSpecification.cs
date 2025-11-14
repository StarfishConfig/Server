using Nerosoft.Euonia.Linq;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Specifications for querying <see cref="Team"/> entities.
/// </summary>
internal static class TeamSpecification
{
	/// <summary>
	/// Specification to check if <see cref="Team"/> Id equals the given id.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static Specification<Team> IdEquals(long id)
	{
		return new DirectSpecification<Team>(t => t.Id == id);
	}

	/// <summary>
	/// Specification to check if <see cref="Team"/> Name equals the given name (case insensitive).
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public static Specification<Team> NameEquals(string name)
	{
		name = name.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Team>(t => t.Name.ToLower() == name);
	}

	/// <summary>
	/// Specification to check if <see cref="Team"/> Name contains the given substring (case insensitive).
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public static Specification<Team> NameContains(string name)
	{
		name = name.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Team>(t => t.Name.ToLower().Contains(name));
	}

	/// <summary>
	/// Specification to check if <see cref="Team"/> Description contains the given substring (case insensitive).
	/// </summary>
	/// <param name="description"></param>
	/// <returns></returns>
	public static Specification<Team> DescriptionContains(string description)
	{
		description = description.Normalize(TextCaseType.Lower);
		return new DirectSpecification<Team>(t => t.Description.ToLower().Contains(description));
	}

	/// <summary>
	/// Specification to check if <see cref="Team"/> matches the given keyword in Name or Description.
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
	/// Specification to check if <see cref="Team"/> has a member with the given userId.
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	public static Specification<Team> HasMember(long userId)
	{
		return new DirectSpecification<Team>(t => t.Members.Any(m => m.UserId == userId));
	}
}