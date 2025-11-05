using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Nerosoft.Euonia.Hosting;
using Nerosoft.Euonia.Modularity;
using Serilog;

namespace Nerosoft.Starfish.Webapi;

/// <summary>
/// The host service module for the Starfish application.
/// </summary>
/// <remarks>
/// This module serves as the entry point for the Starfish host application,
/// configuring and initializing necessary services and components.
/// </remarks>
internal class HostServiceModule : ModuleContextBase
{
    /// <summary>
    /// Configures services for the Starfish host application.
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddLogging(builder =>
        {
            builder.AddConfiguration(Configuration.GetSection("Logging"))
                   .AddConsole()
                   .AddDebug()
                   .AddSerilog();
        });
        context.Services.AddHealthChecks();
        context.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        context.Services.AddOpenApi();
        context.Services.AddAuthentication(Configuration);
        context.Services.AddSwagger();
        context.Services.AddFeatureManagement();
    }

    /// <inheritdoc />
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        app.ServerFeatures.Get<IServerAddressesFeature>();

        app.UseSerilogRequestLogging();
        app.UseForwardedHeaders();
        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); //.AllowCredentials());
    }
}
