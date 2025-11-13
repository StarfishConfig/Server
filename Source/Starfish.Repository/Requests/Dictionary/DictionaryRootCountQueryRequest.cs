using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

public record DictionaryRootCountQueryRequest(DictionaryRootCriteriaDto Criteria)
    : IRequest<int>;
