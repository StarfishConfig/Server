using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Bus.RabbitMq;
using Nerosoft.Euonia.Caching;
using Nerosoft.Euonia.Caching.Memory;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Euonia.Modularity;
using Nerosoft.Euonia.Validation;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Shared;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// The application service module for the Starfish application.
/// </summary>
[DependsOn(typeof(ApplicationModule))]
[DependsOn(typeof(AutomapperModule), typeof(ValidationModule))]
[DependsOn(typeof(RepositoryModule), typeof(DomainServiceModule))]
internal class ApplicationServiceModule : ModuleContextBase
{
    /// <inheritdoc />
    public override void AheadConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AutomapperOptions>(options =>
        {
            options.AddProfile<AccountMapperProfile>();
            options.AddProfile<ProjectMapperProfile>();
            options.AddProfile<SupportMapperProfile>();
        });
    }

    /// <inheritdoc />
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Register<ApplicationServiceContext>();

        context.Services.AddKeyedScoped<IAuthProvider, GithubAuthProvider>(AuthenticationConstant.Provider.Github)
                        .AddKeyedScoped<IAuthProvider, MicrosoftAuthProvider>(AuthenticationConstant.Provider.Microsoft)
                        .AddKeyedScoped<IAuthProvider, GoogleAuthProvider>(AuthenticationConstant.Provider.Google)
                        .AddKeyedScoped<IAuthProvider, FacebookAuthProvider>(AuthenticationConstant.Provider.Facebook);

        ConfigureCachingServices(context.Services);

        ConfigureBusServices(context.Services);
    }

    private void ConfigureCachingServices(IServiceCollection services)
    {
        var provider = Configuration.GetValue<string>("Cache:Provider")?.ToLowerInvariant();
        switch (provider)
        {
            case null:
            case "":
            case "memory":
                services.AddSingleton<ICacheService, MemoryCacheService>();
                break;
            case "redis":
                services.AddSingleton<ICacheService, RedisCacheService>();
                break;
            default:
                throw new NotSupportedException(string.Format(Resources.IDS_ERROR_UNSUPPORTED_CACHE_PROVIDER, provider));
        }

        services.AddDefaultCacheManager<ApplicationServiceContext>();
    }

    private void ConfigureBusServices(IServiceCollection services)
    {
        services.AddServiceBus(config =>
        {
            config.SetConventions(builder =>
            {
                builder.Add<DefaultMessageConvention>();
                builder.Add<AttributeMessageConvention>();
                builder.Add<DomainMessageConvention>();
            });
            config.SetIdentityProvider(jwt =>
            {
                var token = jwt?.Replace("Bearer ", string.Empty);
                ClaimsPrincipal principal;
                if (string.IsNullOrWhiteSpace(token))
                {
                    principal = new ClaimsPrincipal(); //GenericPrincipal(null, null);
                }
                else
                {
                    var validation = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = ClaimTypes.Role,
                        ValidIssuers = [Configuration.GetValue<string>("JwtBearerOptions:TokenIssuer")],
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JwtBearerOptions:TokenKey").ToSha256()))
                    };
                    principal = new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
                    principal ??= new ClaimsPrincipal();
                }

                return principal;

                // var handler = new JwtSecurityTokenHandler().ReadJwtToken(token);
                // var identity = new ClaimsPrincipal(new ClaimsIdentity(handler.Claims, null, null, JwtClaimTypes.Role));
                // return identity;
            });
            config.RegisterHandlers(typeof(ApplicationServiceModule).Assembly);
            var provider = Configuration.GetValue<string>("ServiceBus:Provider")?.ToLower();
            switch (provider)
            {
                case null:
                case "":
                case "inmemory":
                    config.UseInMemory(options =>
                    {
                        options.MultipleSubscriberInstance = Configuration.GetValue<bool>("ServiceBus:InMemory:MultipleSubscriberInstance");
                    });
                    break;
                case "rabbitmq":
                    config.UseRabbitMq(options =>
                    {
                        options.Connection = Configuration.GetValue<string>("ServiceBus:RabbitMq:Connection");
                        options.ExchangeName = Configuration.GetValue<string>("ServiceBus:RabbitMq:ExchangeName");
                        options.ExchangeType = Configuration.GetValue<string>("ServiceBus:RabbitMq:ExchangeType");
                        options.QueueName = Configuration.GetValue<string>("ServiceBus:RabbitMq:QueueName");
                        options.TopicName = Configuration.GetValue<string>("ServiceBus:RabbitMq:TopicName");
                    });
                    break;
                default:
                    throw new NotSupportedException(string.Format(Resources.IDS_ERROR_UNSUPPORTED_SERVICE_BUS_PROVIDER, provider));
            }
        });
    }
}