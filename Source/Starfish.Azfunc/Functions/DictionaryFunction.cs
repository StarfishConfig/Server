using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Nerosoft.Starfish.Azfunc.Functions;

public class DictionaryFunction(ILoggerFactory logger, IDictionaryApplicationService service)
	: HttpFunctionBase<DictionaryFunction>(logger)
{
	public const string ROUTE_PREFIX = "dictionary";
	public const string FUNCTION_NAME = nameof(DictionaryFunction);
}