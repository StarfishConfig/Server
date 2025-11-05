using Nerosoft.Euonia.Application;

namespace Nerosoft.Starfish.Application;

internal sealed class ApplicationServiceContext : ServiceContextBase
{
    /// <inheritdoc/>
    public override bool AutoRegisterApplicationService => true;
}