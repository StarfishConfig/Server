using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

public record DictionaryRootListQueryRequest(DictionaryRootCriteriaDto Criteria, int Skip, int Take)
    : IRequest<IEnumerable<DictionaryRoot>>;