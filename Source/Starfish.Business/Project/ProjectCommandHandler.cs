using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Business;

public class ProjectCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
	: CommandHandlerBase(unitOfWork, factory)
{
}