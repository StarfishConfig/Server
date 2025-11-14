using Nerosoft.Euonia.Linq;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Specifications for querying <see cref="Project"/> entities.
/// </summary>
internal static class ProjectSpecification
{
	/// <summary>
	/// Specification to check all <see cref="Project"/> entities.
	/// </summary>
	public static Specification<Project> All => new DirectSpecification<Project>(t => t.Id > 0);

	/// <summary>
	/// Specification to check if <see cref="Project"/> Id equals the given id.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static Specification<Project> IdEquals(int id)
	{
		return new DirectSpecification<Project>(t => t.Id == id);
	}

	/// <summary>
	/// Specification to check if <see cref="Project"/> Name equals the given name.
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public static Specification<Project> NameEquals(string name)
	{
		return new DirectSpecification<Project>(t => t.Name == name);
	}

	/// <summary>
	/// Specification to check if <see cref="Project"/> Name contains the given substring.
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public static Specification<Project> NameContains(string name)
	{
		return new DirectSpecification<Project>(t => t.Name.Contains(name));
	}

	/// <summary>
	/// Specification to check if <see cref="Project"/> is associated with the given TeamId.
	/// </summary>
	/// <param name="teamId"></param>
	/// <returns></returns>
	public static Specification<Project> TeamIdEquals(long teamId)
	{
		return new DirectSpecification<Project>(t => t.TeamId == teamId);
	}

	/// <summary>
	/// Specification to check if <see cref="Project"/> Description contains the given substring.
	/// </summary>
	/// <param name="description"></param>
	/// <returns></returns>
	public static Specification<Project> DescriptionContains(string description)
	{
		return new DirectSpecification<Project>(t => t.Description.Contains(description));
	}

	/// <summary>
	/// Specification to check if <see cref="Project"/> matches the given keyword in Name or Description.
	/// </summary>
	/// <param name="keyword"></param>
	/// <returns></returns>
	public static Specification<Project> Matches(string keyword)
	{
		ISpecification<Project>[] specifications =
		[
			NameContains(keyword),
			DescriptionContains(keyword)
		];
		return new CompositeSpecification<Project>(PredicateOperator.OrElse, specifications);
	}

	/// <summary>
	/// Specification to check if <see cref="Project"/> is created by the given username.
	/// </summary>
	/// <param name="username"></param>
	/// <returns></returns>
	public static Specification<Project> CreatedBy(string username)
	{
		return new DirectSpecification<Project>(t => t.CreatedBy == username);
	}

	public static Specification<Project> ApplyCriteria(ProjectCriteriaDto criteria)
	{
		Specification<Project> specification = All;
		if (criteria.TeamId > 0)
		{
			specification &= TeamIdEquals(criteria.TeamId);
		}
		if (!string.IsNullOrWhiteSpace(criteria.Keyword))
		{
			specification &= Matches(criteria.Keyword);
		}
		if (!string.IsNullOrWhiteSpace(criteria.Creator))
		{
			specification &= CreatedBy(criteria.Creator);
		}

		{ }

		return specification;
	}
}
