using Nerosoft.Euonia.Linq;

namespace Nerosoft.Starfish.Repository;

public static class DictionaryItemSpecification
{
    public static Specification<DictionaryItem> RootIdEquals(long rootId)
    {
        return new DirectSpecification<DictionaryItem>(t => t.RootId == rootId);
    }

    public static Specification<DictionaryItem> KeyEquals(string key)
    {
        return new DirectSpecification<DictionaryItem>(t => t.Key == key);
    }

    public static Specification<DictionaryItem> KeyContains(string key)
    {
        return new DirectSpecification<DictionaryItem>(t => t.Key.Contains(key));
    }

    public static Specification<DictionaryItem> ValueContains(string value)
    {
        return new DirectSpecification<DictionaryItem>(t => t.Value.Contains(value));
    }

    public static Specification<DictionaryItem> Matches(string keyword)
    {
        ISpecification<DictionaryItem>[] specifications =
        [
            KeyContains(keyword),
            ValueContains(keyword)
        ];

        return new CompositeSpecification<DictionaryItem>(PredicateOperator.OrElse, specifications);
    }
}
