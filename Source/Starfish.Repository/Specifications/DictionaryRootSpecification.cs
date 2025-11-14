using Nerosoft.Euonia.Linq;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Specifications for querying <see cref="DictionaryRoot"/> entities.
/// </summary>
public static class DictionaryRootSpecification
{
	/// <summary>
	/// Specification that matches all DictionaryRoot entities.
	/// </summary>
	public static Specification<DictionaryRoot> All => new DirectSpecification<DictionaryRoot>(t => true);

	/// <summary>
	/// Specification to check if DictionaryRoot Id equals the given id.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static Specification<DictionaryRoot> IdEquals(long id)
	{
		return new DirectSpecification<DictionaryRoot>(t => t.Id == id);
	}

	/// <summary>
	/// Specification to check if DictionaryRoot Code equals the given code.
	/// </summary>
	/// <param name="code"></param>
	/// <returns></returns>
	public static Specification<DictionaryRoot> CodeEquals(string code)
	{
		return new DirectSpecification<DictionaryRoot>(t => t.Code == code);
	}

	/// <summary>
	/// Specification to check if DictionaryRoot Code contains the given code.
	/// </summary>
	/// <param name="code"></param>
	/// <returns></returns>
	public static Specification<DictionaryRoot> CodeContains(string code)
	{
		return new DirectSpecification<DictionaryRoot>(t => t.Code.Contains(code));
	}

	/// <summary>
	/// Specification to check if DictionaryRoot Name contains the given name.
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public static Specification<DictionaryRoot> NameContains(string name)
	{
		return new DirectSpecification<DictionaryRoot>(t => t.Name.Contains(name));
	}

	/// <summary>
	/// Specification to check if DictionaryRoot Code is in the given codes.
	/// </summary>
	/// <param name="codes"></param>
	/// <returns></returns>
	public static Specification<DictionaryRoot> CodeIn(IEnumerable<string> codes)
	{
		return new DirectSpecification<DictionaryRoot>(t => codes.Contains(t.Code));
	}

	/// <summary>
	/// Specification to check if DictionaryRoot Name is in the given names.
	/// </summary>
	/// <param name="names"></param>
	/// <returns></returns>
	public static Specification<DictionaryRoot> NameIn(IEnumerable<string> names)
	{
		return new DirectSpecification<DictionaryRoot>(t => names.Contains(t.Name));
	}

	/// <summary>
	/// Specification to check if DictionaryRoot is enabled (IsValid == true).
	/// </summary>
	/// <returns></returns>
	public static Specification<DictionaryRoot> Enabled()
	{
		return new DirectSpecification<DictionaryRoot>(t => t.IsValid);
	}

	/// <summary>
	/// Specification to check if DictionaryRoot is disabled (IsValid == false).
	/// </summary>
	/// <returns></returns>
	public static Specification<DictionaryRoot> Disabled()
	{
		return new DirectSpecification<DictionaryRoot>(t => !t.IsValid);
	}

	/// <summary>
	/// Specification to check if DictionaryRoot Code or Name contains the given keyword.
	/// </summary>
	/// <param name="keyword"></param>
	/// <returns></returns>
	public static Specification<DictionaryRoot> Matches(string keyword)
	{
		return new DirectSpecification<DictionaryRoot>(t => t.Code.Contains(keyword) || t.Name.Contains(keyword));
	}

	/// <summary>
	/// Applies the given <see cref="DictionaryRootCriteriaDto"/> to build a composite specification.
	/// </summary>
	/// <param name="criteria"></param>
	/// <returns></returns>
	public static Specification<DictionaryRoot> ApplyCriteria(DictionaryRootCriteriaDto criteria)
	{
		Specification<DictionaryRoot> specification = All;
		if (!string.IsNullOrWhiteSpace(criteria.Keyword))
		{
			specification &= Matches(criteria.Keyword);
		}

		switch (criteria.IsValid)
		{
			case true:
				specification &= Matches(criteria.Keyword);
				break;
			case false:
				specification &= Matches(criteria.Keyword);
				break;
		}

		return specification;
	}

	/// <summary>
	/// Specification to check if Remark contains the given remark.
	/// </summary>
	/// <param name="remark"></param>
	/// <returns></returns>
	public static Specification<DictionaryRoot> RemarkContains(string remark)
	{
		return new DirectSpecification<DictionaryRoot>(t => t.Remark.Contains(remark));
	}
}
