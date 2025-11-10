using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Nerosoft.Euonia.Hosting;
using Nerosoft.Euonia.Modularity;
using Nerosoft.Starfish.Application;
using Serilog;

namespace Nerosoft.Starfish.Webapi;

/// <summary>
/// The host service module for the Starfish application.
/// </summary>
/// <remarks>
/// This module serves as the entry point for the Starfish host application,
/// configuring and initializing necessary services and components.
/// </remarks>
[DependsOn(typeof(ApplicationServiceModule))]
internal class HostServiceModule : ModuleContextBase
{
    public override void AheadConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("zh-Hans");
            options.SupportedUICultures = [new CultureInfo("en-US"), new CultureInfo("zh-Hans")];
            options.SupportedCultures = [new CultureInfo("en"), new CultureInfo("zh")];
            options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(httpContext =>
            {
                var language = GetCulture(httpContext);

                return Task.FromResult(new ProviderCultureResult(language, language));
            }));
        });

        context.Services.Configure<JwtAuthenticationOptions>(Configuration.GetSection(nameof(JwtAuthenticationOptions)));
    }

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

    private static string GetCulture(HttpContext context)
    {
        if (context == null)
        {
            return CultureInfo.CurrentCulture.Name;
        }

        if (context.User.FindFirstValue(ClaimTypes.Locality) is { } claimValue)
        {
            return claimValue;
        }

        var requestCulture = context.Request.Headers.AcceptLanguage;

        if (requestCulture.Count <= 0)
        {
            return CultureInfo.CurrentCulture.Name;
        }

        var languages = requestCulture[0]!.Split(',');
        return languages.Length > 0 ? languages[0] : CultureInfo.CurrentCulture.Name;
    }
}