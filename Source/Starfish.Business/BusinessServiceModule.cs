using Nerosoft.Euonia.Modularity;
using Nerosoft.Euonia.Threading.Redis;

namespace Nerosoft.Starfish.Business;

/// <summary>
/// The business service module.
/// </summary>
[DependsOn(typeof(RedisLockModule))]
internal class BusinessServiceModule : ModuleContextBase
{
    /// <inheritdoc/>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddBusinessObject(typeof(BusinessServiceModule).Assembly);
    }
}
