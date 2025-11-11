using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Nerosoft.Starfish.Azfunc.Functions;

public class TeamFunction
{
    private readonly ILogger<TeamFunction> _logger;

    public TeamFunction(ILogger<TeamFunction> logger)
    {
        _logger = logger;
    }

    [Function("TeamFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
