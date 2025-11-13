using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

public record DictionaryItemListQueryRequest(long RootId, DictionaryItemCriteriaDto Criteria, int Skip, int Take)
    : IRequest<IEnumerable<DictionaryItem>>;
