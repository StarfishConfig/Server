using Nerosoft.Euonia.Linq;

namespace Nerosoft.Starfish.Repository;

public static class DictionaryRootSpecification
{
	public static Specification<DictionaryRoot> All => new DirectSpecification<DictionaryRoot>(t => true);

	public static Specification<DictionaryRoot> IdEquals(long id)
	{
		return new DirectSpecification<DictionaryRoot>(t => t.Id == id);
	}

	public static Specification<DictionaryRoot> CodeEquals(string code)
	{
		return new DirectSpecification<DictionaryRoot>(t => t.Code == code);
	}

	public static Specification<DictionaryRoot> CodeContains(string code)
	{
		return new DirectSpecification<DictionaryRoot>(t => t.Code.Contains(code));
	}

	public static Specification<DictionaryRoot> NameContains(string name)
	{
		return new DirectSpecification<DictionaryRoot>(t => t.Name.Contains(name));
	}

	public static Specification<DictionaryRoot> CodeIn(IEnumerable<string> codes)
	{
		return new DirectSpecification<DictionaryRoot>(t => codes.Contains(t.Code));
	}

	public static Specification<DictionaryRoot> NameIn(IEnumerable<string> names)
	{
		return new DirectSpecification<DictionaryRoot>(t => names.Contains(t.Name));
	}

	public static Specification<DictionaryRoot> Enabled()
	{
		return new DirectSpecification<DictionaryRoot>(t => t.IsValid);
	}

	public static Specification<DictionaryRoot> Disabled()
	{
		return new DirectSpecification<DictionaryRoot>(t => !t.IsValid);
	}

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

	public static Specification<DictionaryRoot> RemarkContains(string remark)
	{
		return new DirectSpecification<DictionaryRoot>(t => t.Remark.Contains(remark));
	}
}
