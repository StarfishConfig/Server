using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Nerosoft.Starfish.Azfunc.Functions
{
    public class TeamMemberFunction
    {
        private readonly ILogger<TeamMemberFunction> _logger;

        public TeamMemberFunction(ILogger<TeamMemberFunction> logger)
        {
            _logger = logger;
        }

        [Function("TeamMemberFunction")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
