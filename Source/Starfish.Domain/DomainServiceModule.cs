using Microsoft.Extensions.DependencyInjection;
using Nerosoft.Euonia.Modularity;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// The domain service module.
/// </summary>
internal class DomainServiceModule : ModuleContextBase
{
    /// <inheritdoc/>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddBusinessObject(typeof(DomainServiceModule).Assembly);
    }
}