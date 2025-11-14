using Nerosoft.Euonia.Bus;

namespace Nerosoft.Starfish.Repository;

internal sealed record ProjectDetailQueryRequest(long Id) : IRequest<Project>;
