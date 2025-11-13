using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

public record DictionaryItemCountQueryRequest(long RootId, DictionaryItemCriteriaDto Criteria) 
    : IRequest<int>;
