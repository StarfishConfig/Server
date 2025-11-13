using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

public record DictionaryRootDetailQueryRequest(long Id, params string[] Properties) : IRequest<DictionaryRoot>;
