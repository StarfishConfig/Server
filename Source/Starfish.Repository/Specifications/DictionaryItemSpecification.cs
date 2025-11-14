using Nerosoft.Euonia.Linq;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Specifications for querying <see cref="DictionaryItem"/> entities.
/// </summary>
public static class DictionaryItemSpecification
{
	/// <summary>
	/// Specification to check if <see cref="DictionaryItem"/> RootId equals the given rootId.
	/// </summary>
	/// <param name="rootId"></param>
	/// <returns></returns>
	public static Specification<DictionaryItem> RootIdEquals(long rootId)
	{
		return new DirectSpecification<DictionaryItem>(t => t.RootId == rootId);
	}

	/// <summary>
	/// Specification to check if <see cref="DictionaryItem"/> Key equals the given key.
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public static Specification<DictionaryItem> KeyEquals(string key)
	{
		return new DirectSpecification<DictionaryItem>(t => t.Key == key);
	}

	/// <summary>
	/// Specification to check if <see cref="DictionaryItem"/> Key equals the given key.
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public static Specification<DictionaryItem> KeyContains(string key)
	{
		return new DirectSpecification<DictionaryItem>(t => t.Key.Contains(key));
	}

	/// <summary>
	/// Specification to check if <see cref="DictionaryItem"/> Value contains the given value.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static Specification<DictionaryItem> ValueContains(string value)
	{
		return new DirectSpecification<DictionaryItem>(t => t.Value.Contains(value));
	}

	/// <summary>
	/// Specification to check if any of Key or Value contains the given keyword.
	/// </summary>
	/// <param name="keyword"></param>
	/// <returns></returns>
	public static Specification<DictionaryItem> Matches(string keyword)
	{
		ISpecification<DictionaryItem>[] specifications =
		[
			KeyContains(keyword),
			ValueContains(keyword)
		];

		return new CompositeSpecification<DictionaryItem>(PredicateOperator.OrElse, specifications);
	}

	/// <summary>
	/// Applies the given criteria to build a specification for <see cref="DictionaryItem"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="criteria"></param>
	/// <returns></returns>
	public static Specification<DictionaryItem> ApplyCriteria(long id, DictionaryItemCriteriaDto criteria)
	{
		var specification = RootIdEquals(id);

		if (!string.IsNullOrWhiteSpace(criteria.Keyword))
		{
			specification &= Matches(criteria.Keyword);
		}

		return specification;
	}
}