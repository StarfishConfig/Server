using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

public record DictionaryLookupQueryRequest(IEnumerable<string> Codes, bool? IsValid)
    : IRequest<List<DictionaryFlattenModel>>;