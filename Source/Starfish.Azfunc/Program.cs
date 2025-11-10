using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nerosoft.Starfish.Azfunc;

var builder = FunctionsApplication.CreateBuilder(args);

var azureAppConfiguration = builder.Configuration.GetConnectionString("AzureAppConfiguration");

if (!string.IsNullOrWhiteSpace(azureAppConfiguration))
{
    builder.Configuration.AddAzureAppConfiguration(azureAppConfiguration);
}
else
{
    var environment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");

    if (string.IsNullOrWhiteSpace(environment))
    {
        environment = builder.Environment.EnvironmentName;
    }

    builder.Configuration
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
}

builder.Configuration.AddEnvironmentVariables();

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Services.AddModularityApplication<HostServiceModule>(builder.Configuration);
builder.UseMiddleware<JwtAuthenticationMiddleware>()
       .UseMiddleware<HttpContextAccessorMiddleware>()
       .UseMiddleware<RequestLocalizationMiddleware>()
       .UseMiddleware<ExceptionHandlingMiddleware>();

var host = builder.Build();
host.InitializeApplication();
await host.RunAsync();