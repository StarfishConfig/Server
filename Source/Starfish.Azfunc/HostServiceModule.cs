using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Nerosoft.Euonia.Hosting;
using Nerosoft.Euonia.Modularity;
using Serilog;
using Serilog.Events;

namespace Nerosoft.Starfish.Azfunc;

[DependsOn(typeof(HostingModule), typeof(ApplicationServiceModule))]
internal class HostServiceModule : ModuleContextBase
{
    public override void AheadConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<JwtAuthenticationOptions>(Configuration.GetSection(nameof(JwtAuthenticationOptions)));
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var exceptionlessKey = Configuration["Exceptionless:ApiKey"];

        context.Services.AddLogging(builder =>
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration)
                                                  .MinimumLevel.Override("Worker", LogEventLevel.Warning)
                                                  .MinimumLevel.Override("Host", LogEventLevel.Warning)
                                                  .MinimumLevel.Override("System", LogEventLevel.Error)
                                                  .MinimumLevel.Override("Function", LogEventLevel.Error)
                                                  .MinimumLevel.Override("Azure.Storage.Blobs", LogEventLevel.Error)
                                                  .MinimumLevel.Override("Azure.Core", LogEventLevel.Error)
                                                  .Enrich.FromLogContext()
                                                  .WriteTo.Exceptionless(exceptionlessKey, additionalOperation: null, includeProperties: true, LogEventLevel.Verbose)
                                                  .CreateLogger();

            builder.AddConfiguration(Configuration.GetSection("Logging"))
                   .AddConsole()
                   .AddDebug()
                   .AddSerilog(Log.Logger);
        });

        // context.Services.AddLogging(builder =>
        // {
        //     builder.AddConfiguration(Configuration.GetSection("Logging"))
        //            .AddConsole()
        //            .AddDebug()
        //            .AddExceptionless();
        // });

        context.Services.AddFeatureManagement();
    }
}
