using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Nerosoft.Starfish.Azfunc.Functions;

/// <summary>
/// Project Function
/// </summary>
/// <param name="logger"></param>
/// <param name="service"></param>
public class ProjectFunction(ILoggerFactory logger, IProjectApplicationService service)
	: HttpFunctionBase<ProjectFunction>(logger)
{
}
